import { Component, NgModule } from '@angular/core';
import { Login } from "./login/login";
import { FormsModule } from '@angular/forms';
import { User } from "./user/user";

@Component({
  selector: 'app-root',
  imports: [Login, FormsModule, User],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App {
  protected title = 'myapp';
}
