import { Component, signal, WritableSignal } from '@angular/core';
import { UserModel } from '../models/user-model';
import { UserService } from '../services/user-service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { tap } from 'rxjs';


@Component({
  selector: 'app-create-user',
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './create-user.html',
  styleUrl: './create-user.css'
})
export class CreateUser {
  loading: WritableSignal<boolean> = signal<boolean>(false);

  userForm = new FormGroup({
    firstName: new FormControl(null, Validators.required),
    lastName: new FormControl(null, Validators.required),
    age: new FormControl(null, [Validators.required, Validators.max(100)]),
    email: new FormControl(null, [Validators.required, Validators.email]),
    gender: new FormControl(null, [Validators.required]),
    phone: new FormControl(null, [Validators.required]),
    birthDate: new FormControl(null, [Validators.required]),
  })


  public get firstName(): any {
    return this.userForm.get("firstName")
  }
  public get lastName(): any {
    return this.userForm.get("lastName")
  }

  public get age(): any {
    return this.userForm.get("age")
  }
  public get email(): any {
    return this.userForm.get("email")
  }
  public get gender(): any {
    return this.userForm.get("gender")
  }
  public get phone(): any {
    return this.userForm.get("phone")
  }

  public get birthDate(): any {
    return this.userForm.get("birthDate")
  }

  constructor(private userService: UserService) {
  }
  
  onSubmit() {
    if (this.userForm.invalid) return;

    const newuser = this.userForm.value as any as UserModel;

    this.loading.set(true);

    this.userService.createUser(newuser).subscribe({
      next: () => {
        this.loading.set(false);
        alert("User added successfully");
      },
      error: (err) => {
        console.error(err);
        this.loading.set(false);
      }
    });
  }
}
