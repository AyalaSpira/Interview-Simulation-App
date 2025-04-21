import { Component, OnInit, inject } from '@angular/core';
import { ServiceService } from '../../services/service.service';
import { MatDialog, MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule, SlicePipe } from '@angular/common';
import { AuthService } from '../../services/auth.service';  // נתיב זה משתנה לפי מיקום הקובץ שלך
import { Inject } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-track-users',
  standalone: true,
  imports: [
    CommonModule,
    MatExpansionModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    SlicePipe,
  ],
  templateUrl: './track-users.component.html',
  styleUrls: ['./track-users.component.css'],
})
export class TrackUsersComponent implements OnInit {

  resumes: any[] = [];
  reports: any[] = [];
  users: any[] = [];
  resumeContents: { [url: string]: string } = {};
  reportContents: { [url: string]: string } = {};

  private dataService = inject(ServiceService);
  private dialog = inject(MatDialog);
  private authService = inject(AuthService);
  private router = inject(Router);

  ngOnInit(): void {
    this.loadResumes();
    this.loadReports();
    this.loadUsers();
    if (!this.authService.isAdmin()) {
      this.router.navigate(['/access-denied']);
    }
  }
  

  loadResumes(): void {
    this.dataService.getResumes().subscribe(resumes => {
      this.resumes = resumes;
    });
  }

  loadReports(): void {
    this.dataService.getReports().subscribe(reports => {
      this.reports = reports;
    });
  }

  loadUsers(): void {
    this.authService.getUsers().subscribe(users => {
      this.users = users;
    });
  }
  
  onDeleteUser(userId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '250px',
      data: { message: 'האם אתה בטוח שברצונך למחוק את המשתמש?' }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.authService.deleteUser(userId).subscribe(() => {
          this.loadUsers();
        });
      }
    });
  }
  

  loadContent(url: string, type: 'resume' | 'report'): void {
    this.dataService.getFileContent(url).subscribe(content => {
      if (type === 'resume') {
        this.resumeContents[url] = content;
      } else {
        this.reportContents[url] = content;
      }
    });
  }

  deleteFile(url: string, type: 'resume' | 'report'): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '250px',
      data: { message: 'האם אתה בטוח שברצונך למחוק את הקובץ?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.dataService.deleteFile(url, type).subscribe(() => {
          if (type === 'resume') {
            this.resumes = this.resumes.filter(resume => resume !== url);
            delete this.resumeContents[url];
          } else {
            this.reports = this.reports.filter(report => report !== url);
            delete this.reportContents[url];
          }
        });
      }
    });
  }
}

@Component({
  selector: 'confirm-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule],
  template: `
    <h1 mat-dialog-title>אישור מחיקה</h1>
    <div mat-dialog-content>
      <p>{{ data.message }}</p>
    </div>
    <div mat-dialog-actions>
      <button mat-button (click)="onNoClick()">לא</button>
      <button mat-button [mat-dialog-close]="true" cdkFocusInitial>כן</button>
    </div>
  `,
})
export class ConfirmDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any // הוספת ה־Inject
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}