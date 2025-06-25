import { Component, inject } from '@angular/core';
import { LoginModel } from '../models/loginmodel';
import { AuthService } from '../service/authservice';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TextValidator } from '../misc/text-validator';

@Component({
  selector: 'app-login',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  public login: LoginModel = new LoginModel();
  public loginForm: FormGroup = new FormGroup({
    un: new FormControl(null, Validators.required),
    pass: new FormControl(null, [Validators.required, TextValidator()])
  });
  public authService: AuthService = inject(AuthService);
  private route: Router = inject(Router);

  public get un(): any {
    return this.loginForm.get("un");
  }

  public get pass(): any {
    return this.loginForm.get("pass");
  }

  onSubmit() {
    if (this.loginForm.invalid)
      return;
    console.log(this.un);
    
    this.authService.validateUserLogin(this.login);
    this.route.navigateByUrl(`/home/${this.un.value}/products`);
  }
}