import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; // ← טופס דו-כיווני

import { MatCardModule } from '@angular/material/card'; // ייבוא MatCardModule
import { MatTableModule } from '@angular/material/table'; // ייבוא MatTableModule
import { MatIconModule } from '@angular/material/icon'; // ייבוא MatIconModule/ ייבוא MatIconModule
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
@Component({
  selector: 'app-manage-users',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatIconModule,
    MatCardModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './manage-users.component.html',
  styleUrls: ['./manage-users.component.css']
})
export class ManageUsersComponent implements OnInit {
  users: any[] = [];
  displayedColumns: string[] = ['email', 'username', 'actions']; // עמודות שהצגנו בטבלה

  newUser = { email: '', username: '' }; // אובייקט חדש למשתמש
  editedUser = { email: '', username: '' }; // אובייקט לעריכה
  editingUserId: number | null = null; // מזהה המשתמש לעריכה
  
  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.http.get<any[]>('https://interview-simulation-app-ar3c.onrender.com/api/admin/all-users/')
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
    this.http.delete(`https://interview-simulation-app-ar3c.onrender.com/api/admin/delete/${id}`)
      .subscribe(() => {
        this.loadUsers(); // ריענון הרשימה אחרי מחיקה
      });
  }

  addUser() {
    this.http.post('https://interview-simulation-app-ar3c.onrender.com/api/admin/add-user', this.newUser)
      .subscribe(() => {
        this.newUser = { email: '', username: '' };
        this.loadUsers();
      });
  }

  editUser(user: any) {
    this.editingUserId = user.id;
    this.editedUser = { email: user.email, username: user.username };
  }
  cancelEdit() {
    this.editingUserId = null;
    this.editedUser = { email: '', username: '' };
  }

  updateUser(id: number) {
    this.http.put(`https://interview-simulation-app-ar3c.onrender.com/api/admin/update-user/${id}`, this.editedUser)
      .subscribe(() => {
        this.editingUserId = null;
        this.loadUsers();
      });
  }

  

}












