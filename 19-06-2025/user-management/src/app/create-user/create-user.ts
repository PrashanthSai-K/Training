import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { UserModel } from '../models/user-model';
import { UserService } from '../services/user-service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UsernameValidator } from '../validators/username-validator';
import { PasswordValidator } from '../validators/password-validator';
import {MatSnackBar} from '@angular/material/snack-bar';
import { Router } from '@angular/router';



@Component({
  selector: 'app-create-user',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './create-user.html',
  styleUrl: './create-user.css'
})
export class CreateUser implements OnInit {
  loading: WritableSignal<boolean> = signal<boolean>(false);
  snackBar = inject(MatSnackBar);
  router = inject(Router);

  userForm = new FormGroup({
    firstName: new FormControl(null, Validators.required),
    lastName: new FormControl(null, Validators.required),
    email: new FormControl(null, [Validators.required, Validators.email]),
    username: new FormControl(null, [Validators.required, UsernameValidator()]),
    password: new FormControl(null, [Validators.required, PasswordValidator()]),
    confirmPassword: new FormControl(null, [Validators.required]),
    role: new FormControl("user", [Validators.required]),
  })

  public get firstName(): any {
    return this.userForm.get("firstName")
  }
  public get lastName(): any {
    return this.userForm.get("lastName")
  }

  public get email(): any {
    return this.userForm.get("email")
  }
  public get username(): any {
    return this.userForm.get("username")
  }
  public get password(): any {
    return this.userForm.get("password")
  }
  public get confirmPassword(): any {
    return this.userForm.get("confirmPassword")
  }

  public get role(): any {
    return this.userForm.get("role")
  }

  constructor(private userService: UserService) {
  }

  ngOnInit(): void {
    this.userService.users$.subscribe({
      next: (data) => console.log(data)
    })
  }

  onSubmit() {    
    if (this.userForm.invalid ) return;
    if(this.userForm.get('password')?.value !== this.userForm.get('confirmPassword')?.value)
        return;
    const newuser = this.userForm.value as any as UserModel;

    this.loading.set(true);
    this.userService.createUser(newuser);
    this.loading.set(false);
    this.snackBar.open("User created successfully");
    this.router.navigateByUrl("/users");
  }
}
