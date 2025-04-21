

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { ChartModule } from 'primeng/chart';
import { HttpClientModule } from '@angular/common/http';
import { CardModule } from 'primeng/card';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

interface UserSummary {
  mark: number;
}

interface User {
  email: string;
  summary: UserSummary;
}

@Component({
  selector: 'app-user-statistics',
  standalone: true,
  imports: [CommonModule, HttpClientModule, CardModule, ChartModule],
  templateUrl: './user-statistics.component.html',
  styleUrls: ['./user-statistics.component.css']
})
export class UserStatisticsComponent implements OnInit {
  chartData: any;
  chartOptions: any;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    // נתוני דמו במקום קריאה ל-API
    const users = [
      { email: 'user1@example.com', summary: { mark: 85 } },
      { email: 'user2@example.com', summary: { mark: 10 }},
      { email: 'user3@example.com', summary: { mark: 70 } },
      // הוסף עוד משתמשים דמויים כרצונך
    ];
  
    const labels = users.map((user: any) => user.email);
    const scores = users.map((user: any) => this.calculateMark(user.summary));
  
    this.chartData = {
      labels,
      datasets: [
        {
          label: 'User Marks',
          data: scores,
          backgroundColor: '#42A5F5'
        }
      ]
    };
  
    this.chartOptions = {
      responsive: true,
      maintainAspectRatio: false
    };
  }
  

  private loadUsers(): void {
    this.authService.getUsers()
      .pipe(catchError((error) => {
        console.error('Error loading users', error);
        return [];
      }))
      .subscribe(users => {
        if (users && users.length > 0) {
          this.prepareChartData(users);
        }
      });
  }

  private prepareChartData(users: User[]): void {
    const labels = users.map(user => user.email);
    const scores = users.map(user => this.calculateMark(user.summary));

    this.chartData = {
      labels,
      datasets: [
        {
          label: 'User Marks',
          data: scores,
          backgroundColor: '#42A5F5'
        }
      ]
    };

    this.chartOptions = {
      responsive: true,
      maintainAspectRatio: false
    };
  }

  calculateMark(summary: UserSummary): number {
    if (!summary) return 0;
    const value = Number(summary.mark);
    return isNaN(value) ? 0 : Math.min(Math.max(value, 0), 100);
  }
}

