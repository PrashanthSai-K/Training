import { Component, inject } from '@angular/core';
import { LoginModel } from '../models/loginmodel';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../service/authservice';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  public login: LoginModel = new LoginModel();
  public authService: AuthService = inject(AuthService);

  onSubmit() {
    var result: boolean = this.authService.AuthenticateUser(this.login);
    if (!result) {
      localStorage.removeItem("username");
      sessionStorage.removeItem("username");
      alert("Invalid user");
    }
  }
}