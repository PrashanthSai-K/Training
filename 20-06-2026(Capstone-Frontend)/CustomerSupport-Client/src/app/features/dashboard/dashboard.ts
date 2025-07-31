import { Component, OnInit, signal } from '@angular/core';
import { DashboardService } from '../../core/services/dashboard-service';
import { AuthService } from '../../core/services/auth-service';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AsyncPipe, CommonModule } from '@angular/common';
import { provideNoopAnimations } from '@angular/platform-browser/animations';


@Component({
  selector: 'app-dashboard',
  imports: [NgxChartsModule, AsyncPipe, CommonModule],
  providers: [provideNoopAnimations()],
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
      complete: () => {
        this.isLoading.set(false);
      }
    })
  }

  transformToNgxChartFormat(data: any) {
    const grouped = new Map<string, Map<string, number>>();
    const allDates = new Set<string>();

    // 1. Group data and collect all unique dates
    for (const item of data) {
      const date = new Date(item.date).toISOString().slice(0, 10);
      const status = item.status === 'Deleted' ? 'Closed' : item.status === 'Active' ? 'Open' : item.status;

      allDates.add(date);

      if (!grouped.has(status)) {
        grouped.set(status, new Map());
      }

      const statusMap = grouped.get(status)!;
      statusMap.set(date, (statusMap.get(date) || 0) + 1);
    }

    // 2. Convert Set to sorted array of dates (descending or ascending as needed)
    const sortedDates = Array.from(allDates).sort((a, b) => a.localeCompare(b)); 
    
    // 3. Create series for each status with all dates (fill missing with 0)
    const chartData = Array.from(grouped.entries()).map(([status, dateMap]) => ({
      name: status,
      series: sortedDates.map(date => ({
        name: date,
        value: dateMap.get(date) || 0
      }))
    }));

    return chartData;
  }

}
