import { Component, OnInit, ViewChild } from '@angular/core';
import { UserTable } from "../user-table/user-table";
import { UserModel } from '../models/user-model';
import { UserService } from '../services/user-service';
import { Chart, ChartConfiguration, ChartEvent, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';


@Component({
  selector: 'app-dashboard',
  imports: [UserTable, BaseChartDirective],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

  users: UserModel[] | null = null;
  femaleCount: number = 0;
  maleCount: number = 0;
  pieChartData: any;
  barChartData: any;
  public pieChartType: ChartType = 'pie';
  public barChartType: ChartType = 'bar';

  constructor(private userService: UserService) {
  }
  stateCounts(users: UserModel[]): Record<string, number> {
    return users.reduce((acc, user) => {
      const st = user.address.state || 'Unknown';
      acc[st] = (acc[st] || 0) + 1;
      return acc;
    }, {} as Record<string, number>);
  }

  ngOnInit(): void {
    this.userService.getAllUsers().subscribe({
      next: (data: any) => {
        this.users = data.users as UserModel[];
        this.femaleCount = this.users.filter(u => u.gender === 'female').length;
        this.maleCount = this.users.filter(u => u.gender === 'male').length;

        this.pieChartData = {
          labels: ['Male', 'Female'],
          datasets: [{ data: [this.maleCount, this.femaleCount] }]
        };

        const counts = this.stateCounts(this.users);
        this.barChartData = {
          labels: Object.keys(counts),
          datasets: [{
            data: Object.values(counts),
            label: 'Users by State',
          }]
        };
      },
      error: err => console.error(err)
    });
  }

  public chartOptions: ChartConfiguration['options'] = {
    responsive:true,
    maintainAspectRatio:false,
    plugins: {
      legend: {
        display: true,
        position: 'bottom',
      },
    },
  };

}
