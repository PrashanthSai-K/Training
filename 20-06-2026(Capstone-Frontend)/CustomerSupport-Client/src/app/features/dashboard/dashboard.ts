import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../core/services/dashboard-service';
import { AuthService } from '../../core/services/auth-service';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

  summary: any = {};

  constructor(private authService: AuthService, private dashboardService: DashboardService) {

  }
  ngOnInit(): void {
    this.authService.getUser().subscribe({
      next: (user) => {
        if (user?.role == "Admin")
          this.dashboardService.getAdminSummary().subscribe({
            next: (data) => {
              console.log(this.summary);
              
              this.summary = data;
            }
          })
      }
    })
  }
}
