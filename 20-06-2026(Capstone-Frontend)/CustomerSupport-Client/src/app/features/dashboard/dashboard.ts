import { Component, OnInit, signal } from '@angular/core';
import { DashboardService } from '../../core/services/dashboard-service';
import { AuthService } from '../../core/services/auth-service';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AsyncPipe, CommonModule } from '@angular/common';


@Component({
  selector: 'app-dashboard',
  imports: [NgxChartsModule, AsyncPipe, CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

  summary: any = {};
  trends: any = [];
  isLoading = signal(true);

  constructor(public authService: AuthService, private dashboardService: DashboardService) {

  }

  ngOnInit(): void {
    this.authService.getUser().subscribe({
      next: (user) => {
        if (user?.role == "Admin") {
          this.dashboardService.getAdminSummary().subscribe({
            next: (data) => {
              this.summary = data;
            }
          })
          this.dashboardService.getAdminChatTrend().subscribe({
            next: (data) => {
              console.log(data);
              this.trends = this.transformToNgxChartFormat(data);
            }
          })
        }
        if (user?.role == "Agent" || user?.role == "Customer") {
          this.dashboardService.getUserSummary().subscribe({
            next: (data) => {
              this.summary = data;
            }
          });
          this.dashboardService.getUserChatTrend().subscribe({
            next: (data) => {
              this.trends = this.transformToNgxChartFormat(data);
            }
          })
        }
      },
      complete:()=>{
        this.isLoading.set(false);
      }
    })
  }

  transformToNgxChartFormat(data: any) {
    const grouped = new Map<string, Map<string, number>>();

    for (const item of data) {
      const date = new Date(item.date).toISOString().slice(0, 10);

      if (!grouped.has(item.status)) {
        grouped.set(item.status, new Map());
      }

      const statusMap = grouped.get(item.status)!;
      statusMap.set(date, (statusMap.get(date) || 0) + 1);
    }

    const chartData = Array.from(grouped.entries()).map(([status, dateMap]) => ({
      name: status,
      series: Array.from(dateMap.entries())
        .sort(([a], [b]) => a.localeCompare(b))
        .map(([date, value]) => ({
          name: date,
          value: value
        }))
    }));

    return chartData;
  }

}
