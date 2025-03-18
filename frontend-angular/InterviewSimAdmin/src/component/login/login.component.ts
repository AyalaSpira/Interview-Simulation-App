import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatCardModule,
    RouterModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm!: FormGroup;
  errorMessage: string = '';
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { // הוספת Router
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  getEmailErrorMessage() {
    const emailControl = this.loginForm.controls['email'];
    if (emailControl.hasError('required')) {
      return 'Email is required';
    }
    if (emailControl.hasError('email')) {
      return 'Enter a valid email';
    }
    return '';
  }

  getPasswordErrorMessage() {
    const passwordControl = this.loginForm.controls['password'];
    if (passwordControl.hasError('required')) {
      return 'Password is required';
    }
    if (passwordControl.hasError('minlength')) {
      return 'Password must be at least 6 characters long';
    }
    return '';
  }

  
  onSubmit() {
    if (this.loginForm.valid) {
      console.log('Login Data:', this.loginForm.value);
      this.authService.login(this.loginForm.value).subscribe( // שימוש ב-loginForm.value
        (response: any) => {
          console.log('Login successful', response);
          localStorage.setItem('token', response.token);
          localStorage.setItem('role', response.role); // שמירת ה-role
          this.router.navigate(['/home']); // הפניה לדף הבית
        },
        (error: any) => {
          console.error('Login failed', error);
          this.errorMessage = error.error.message || 'An error occurred. Please try again.';
        }
      );
    }}
}

