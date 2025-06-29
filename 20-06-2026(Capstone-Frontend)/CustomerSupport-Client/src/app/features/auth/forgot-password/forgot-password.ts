import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RouterLink } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-forgot-password',
  imports: [CommonModule, FormsModule, LucideAngularModule, ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.css'
})
export class ForgotPassword {
  isSubmitting = signal(false);
  forgotForm = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
  })

  public get email(): any {
    return this.forgotForm.get("email");
  }

  private _snackBar = inject(MatSnackBar);

  constructor(private authService: AuthService) {

  }

  onSubmit() {
    if (this.forgotForm.invalid)
      return this._snackBar.open("Enter valid details", "", {
        duration: 1000
      });
    this.isSubmitting.set(true);
    this.authService.forgotPassword(this.email.value).subscribe({
      next: (data) => {
        this._snackBar.open("Reset link sent to your email account successfully", "", {
          duration: 1000
        })
        this.isSubmitting.set(false);
      },
      error: (err) => {
        this.isSubmitting.set(false);
        if (err?.error?.statusCode == 404)
          this._snackBar.open("Mail not found", "", {
            duration: 1000
          })
      }
    })
    return;
  }
}
