import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card'; // ייבוא MatCardModule
import { MatTableModule } from '@angular/material/table'; // ייבוא MatTableModule
import { MatIconModule } from '@angular/material/icon'; // ייבוא MatIconModule/ ייבוא MatIconModule

@Component({
  selector: 'app-manage-users',
  standalone: true,
  imports: [CommonModule ,MatIconModule, MatCardModule, MatTableModule], // הוספת MatCardModule ו-MatTableModule
  templateUrl: './manage-users.component.html',
  styleUrls: ['./manage-users.component.css']
})
export class ManageUsersComponent implements OnInit {
  users: any[] = [];
  displayedColumns: string[] = ['email', 'username', 'actions']; // עמודות שהצגנו בטבלה

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.http.get<any[]>('https://interview-simulation-app-ar3c.onrender.com/api/users')
      .subscribe({
        next: data => {
          this.users = data;
        },
        error: err => {
          console.error('Loading failed, using mock data');
          this.users = [
            { id: 1, email: 'mock1@example.com', username: 'mock1' },
            { id: 2, email: 'mock2@example.com', username: 'mock2' }
          ];
        }
      });
  }

  deleteUser(id: number) {
    this.http.delete(`https://interview-simulation-app-ar3c.onrender.com/api/users/${id}`)
      .subscribe(() => {
        this.loadUsers(); // ריענון הרשימה אחרי מחיקה
      });
  }
}
