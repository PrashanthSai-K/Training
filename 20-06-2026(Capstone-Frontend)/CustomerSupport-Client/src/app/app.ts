import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from "./shared/navbar/navbar";
import { AuthService } from './core/services/auth-service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { AdminSidebar } from './shared/admin-sidebar/admin-sidebar';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, AsyncPipe, AdminSidebar, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'CustomerSupport-Client';

  constructor(public authService:AuthService){
  }
}
