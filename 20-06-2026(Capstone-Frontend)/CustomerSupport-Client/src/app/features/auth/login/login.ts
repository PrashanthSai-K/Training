import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { AuthService } from '../../../core/services/auth-service';
import { LoginModel } from '../../../core/models/login';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PasswordValidator } from '../../../core/validators/password-validator';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-login',
  imports: [LucideAngularModule, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  isSubmitting = signal(false);
  loginForm = new FormGroup({
    username: new FormControl("", [Validators.required, Validators.email]),
    password: new FormControl("", [Validators.required, PasswordValidator()]),
  })

  public get username(): any {
    return this.loginForm.get("username");
  }

  public get password(): any {
    return this.loginForm.get("password");
  }

  private _snackBar = inject(MatSnackBar);

  constructor(private router: Router, private authService: AuthService) {
    authService.currentUser$.subscribe({
      next: (value) => {
        if (value)
          router.navigateByUrl("/chat");
      },
    })
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      this._snackBar.open("Enter valid credentials");
      return;
    }
    this.isSubmitting.set(true);
    this.authService.loginUser(this.loginForm.value as LoginModel).subscribe({
      next: (data) => {
        this.isSubmitting.set(false);
        this._snackBar.open("Logged in successfully", "", {
          duration: 1000
        });
        this.router.navigateByUrl("/chat");
      },
      error: (err) => {
        this.isSubmitting.set(false)
        if (err?.error?.statusCode)
          this._snackBar.open(err.error.message, "", {
            duration: 2000
          });
        else if (err?.error?.errors) {
          this._snackBar.open("Enter valid credentials", "", {
            duration: 2000
          })
        } else {
          console.log(err);
        }
      }
    });
  }

  redirectRegister() {
    this.router.navigateByUrl("/register");
  }
}
