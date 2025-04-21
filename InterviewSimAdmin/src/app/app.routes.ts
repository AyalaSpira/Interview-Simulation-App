import { Routes } from '@angular/router';
import { HomeComponent } from '../component/home/home.component';
import { LoginComponent } from '../component/login/login.component';
import { TrackUsersComponent } from '../component/track-users/track-users.component';
import { ManageUsersComponent } from '../component/manage-users/manage-users.component';
import { AuthGuard } from './auth.guard';
import { UserStatisticsComponent } from '../component/user-statistics/user-statistics.component'; // עדכן את הנתיב למיקום הנכון

export const routes: Routes = [
  { path: '', loadComponent: () => import('../component/home/home.component').then(c => c.HomeComponent) },
  { path: 'login', loadComponent: () => import('../component/login/login.component').then(c => c.LoginComponent) },
  { path: 'track-users', loadComponent: () => import('../component/track-users/track-users.component').then(c => c.TrackUsersComponent), canActivate: [AuthGuard] },
  { path: 'manage-users', loadComponent: () => import('../component/manage-users/manage-users.component').then(c => c.ManageUsersComponent), canActivate: [AuthGuard] },
  { path: 'statistics', loadComponent: () => import('../component/user-statistics/user-statistics.component').then(c => c.UserStatisticsComponent), canActivate: [AuthGuard] },

  { path: '**', redirectTo: '' } // נתיב עבור כל נתיב לא קיים
];
