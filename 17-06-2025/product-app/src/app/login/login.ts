import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoginModel } from '../model/login-model';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { AuthService } from '../service/auth-service';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  login: LoginModel = new LoginModel();
  router: Router = inject(Router);
  authService: AuthService = inject(AuthService);

  onSubmit() {
    this.authService.authenticateUser();
    this.router.navigateByUrl("");
  }
}
