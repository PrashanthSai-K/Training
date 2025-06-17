import { Component, inject } from '@angular/core';
import { LoginModel } from '../models/loginmodel';
import { AuthService } from '../service/authservice';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  public login: LoginModel = new LoginModel();
  public authService: AuthService = inject(AuthService);
  private route: Router = inject(Router);

  onSubmit() {
    this.authService.validateUserLogin(this.login);
    this.route.navigateByUrl(`/home/${this.login.username}/products`);
  }
}