// import { HttpClient } from '@angular/common/http';
// import { Injectable } from '@angular/core';
// import { Observable } from 'rxjs';
// import { JwtHelperService } from '@auth0/angular-jwt'; // הוספתי את ה-helper
// import { User } from '../models/user';

// @Injectable({
//   providedIn: 'root'
// })
// export class AuthService {
//   private apiUrl = 'https://interview-simulation-app-ar3c.onrender.com/api/auth/login';

//   private jwtHelper = new JwtHelperService(); // הוספתי את ה-helper

//   constructor(private http: HttpClient) { }

//   logout() {
//     localStorage.removeItem('token');
//     localStorage.removeItem('role');
//     localStorage.removeItem('userId');
//   }

//   isAuthenticated(): boolean {
//     return !!localStorage.getItem('token');
//   }

//   login(credentials: any): Observable<User> {
//     return this.http.post<User>(`${this.apiUrl}/login`, credentials);
//   }

//   getToken(): string | null {
//     return localStorage.getItem('token');
//   }

//   getUserId(): number | null {
//     const userId = localStorage.getItem('userId');
//     if (userId) {
//       const parsedUserId = parseInt(userId, 10);
//       if (!isNaN(parsedUserId)) {
//         return parsedUserId;
//       } else {
//         console.error('Invalid userId in LocalStorage:', userId);
//         return null;
//       }
//     }
//     return null;
//   }

//   // פונקציה לבדוק אם המשתמש הוא מנהל
//   isAdmin(): boolean {
//     const token = this.getToken();
//     if (token) {
//       const decodedToken = this.jwtHelper.decodeToken(token);
//       return decodedToken.role === 'Admin';  // בודק אם יש לו תפקיד Admin ב-token
//     }
//     return false;
//   }
// }

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // deleteUser(userId: number) {
  //   throw new Error('Method not implemented.');
  // }
  private apiUrl = 'https://interview-simulation-app-ar3c.onrender.com/api/auth';

  private jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) { }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    localStorage.removeItem('userId');
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  login(credentials: any): Observable<User> {
    return this.http.post<User>('https://interview-simulation-app-ar3c.onrender.com/api/admin', credentials);
  }

  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/users`);
  }
  
  deleteUser(userId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/users/${userId}`);
  }
  
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserId(): number | null {
    const userId = localStorage.getItem('userId');
    if (userId) {
      const parsedUserId = parseInt(userId, 10);
      if (!isNaN(parsedUserId)) {
        return parsedUserId;
      } else {
        console.error('Invalid userId in LocalStorage:', userId);
        return null;
      }
    }
    return null;
  }

  isAdmin(): boolean {
    const token = this.getToken();
    if (token) {
      try{
        const decodedToken = this.jwtHelper.decodeToken(token);
        return decodedToken?.role === 'Admin';
      }catch (error) {
        console.error('Error decoding token:', error);
      }

    }
    return false;
  }
}
