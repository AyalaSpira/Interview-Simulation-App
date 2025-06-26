import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {

  private apiUrl = 'http://localhost:5000/api/Admin'; // החלף בכתובת ה-API שלך
  private http = inject(HttpClient);

  getResumes(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/resumes`).pipe(catchError(this.handleError));;
  }

  getReports(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/reports`).pipe(catchError(this.handleError));;
  }

  getFileContent(url: string): Observable<string> {
    return this.http.get(url, { responseType: 'text' }).pipe(catchError(this.handleError));;
  }

  deleteFile(fileUrl: string, fileType: string): Observable<any> {
    const params = new HttpParams()
      .set('fileUrl', fileUrl)
      .set('fileType', fileType);
    return this.http.delete(`${this.apiUrl}/delete-file`, { params }).pipe(catchError(this.handleError));;
  }

  getAllUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all-users`).pipe(catchError(this.handleError));;
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${userId}`).pipe(catchError(this.handleError));;
  }
  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    return throwError(
      'Something bad happened; please try again later.');
  }


}