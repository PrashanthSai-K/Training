import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';
import { CommonModule } from '@angular/common';
import { LucideAngularModule } from 'lucide-angular';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-reset-password',
  imports: [CommonModule, ReactiveFormsModule, LucideAngularModule],
  templateUrl: './reset-password.html',
  styleUrl: './reset-password.css'
})
export class ResetPassword {
  isSubmitting = false;
  token = '';
  email = '';

  resetForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.route.queryParams.subscribe({
      next: (params) => {
        this.token = params['token'];
        this.email = params['email'];
      }
    })
    console.log(this.token);

    this.resetForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(12)]],
      confirmPassword: ['']
    }, {
      validators: this.matchPasswords
    });
  }
  private _snackBar = inject(MatSnackBar);

  get password() {
    return this.resetForm.get('password')!;
  }

  matchPasswords(group: FormGroup) {
    return group.get('password')!.value === group.get('confirmPassword')!.value
      ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.resetForm.invalid) return;

    this.isSubmitting = true;
    const password = this.resetForm.value.password;

    this.authService.resetPassword(this.token, this.email, password).subscribe({
      next: () => {
        this._snackBar.open('Password reset successfully.', "", {
          duration: 1000
        });
        this.router.navigate(['/login']);
      },
      error: () => {
        this._snackBar.open('Password reset failed.', "", {
          duration: 1000
        });
        this.isSubmitting = false;
      }
    });
  }

}
