import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private apiUrl = 'your-backend-api-url/api/Admin'; // החלף בכתובת ה-API שלך
  private http = inject(HttpClient);

  getResumes(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/resumes`);
  }

  getReports(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/reports`);
  }

  getFileContent(url: string): Observable<string> {
    return this.http.get(url, { responseType: 'text' });
  }

  deleteFile(fileUrl: string, fileType: string): Observable<any> {
    const params = new HttpParams()
      .set('fileUrl', fileUrl)
      .set('fileType', fileType);
    return this.http.delete(`${this.apiUrl}/delete-file`, { params });
  }
}