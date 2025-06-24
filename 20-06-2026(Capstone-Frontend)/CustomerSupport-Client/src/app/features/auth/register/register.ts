import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { PasswordValidator } from '../../../core/validators/password-validator';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomerService } from '../../../core/services/customer-service';
import { CustomerModel } from '../../../core/models/register';

@Component({
  selector: 'app-register',
  imports: [LucideAngularModule, ReactiveFormsModule, CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  isSubmitting = signal(false);

  registerForm = new FormGroup({
    name: new FormControl("", [Validators.required]),
    email: new FormControl("", [Validators.required, Validators.email]),
    phone: new FormControl("", [Validators.required, Validators.pattern(/^(0|[7-9])\d{9}$/)]),
    password: new FormControl("", [Validators.required, PasswordValidator()])
  })
  private _snackBar = inject(MatSnackBar);

  public get name(): any {
    return this.registerForm.get("name")
  }

  public get email(): any {
    return this.registerForm.get("email")
  }

  public get phone(): any {
    return this.registerForm.get("phone")
  }

  public get password(): any {
    return this.registerForm.get("password")
  }


  constructor(private router: Router, private customerService: CustomerService) {
  }

  onSubmit() {
    console.log(this.registerForm.value);
    if (this.registerForm.invalid) {
      this._snackBar.open("Enter valid data in the form");
      return;
    }
    this.isSubmitting.set(true);

    this.customerService.registerCustomer(this.registerForm.value as CustomerModel).subscribe({
      next: (data) => {
        this.isSubmitting.set(false);
        this._snackBar.open("Signed up successfully.", "", {
          duration:2000
        });
        this.router.navigateByUrl("/login");
      },
      error: (err) => {
        this.isSubmitting.set(false);
        console.log(err);
      }
    })
  }

  redirectLogin() {
    this.router.navigateByUrl("/login");
  }
}
