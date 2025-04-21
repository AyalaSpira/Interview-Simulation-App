import { Router, RouterModule } from '@angular/router';
import { Component } from '@angular/core';
import {  RouterOutlet } from '@angular/router'; // ייבא RouterOutlet
import { HomeComponent } from '../component/home/home.component';
import { LoginComponent } from '../component/login/login.component';
import { TrackUsersComponent } from '../component/track-users/track-users.component';
import { ManageUsersComponent } from '../component/manage-users/manage-users.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common'; // ייבא CommonModule

@Component({
  selector: 'app-root',
  standalone: true, // הוסף standalone: true
  imports: [
    RouterOutlet, 
    RouterModule,// הוסף RouterOutlet
    HomeComponent,
    LoginComponent,
    TrackUsersComponent,
    ManageUsersComponent,
    MatToolbarModule,
    MatButtonModule,
    CommonModule
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title(title: any) {
    throw new Error('Method not implemented.');
  }

  constructor(private router: Router) { }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}