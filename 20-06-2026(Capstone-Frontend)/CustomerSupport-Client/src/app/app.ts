import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Navbar } from "./shared/navbar/navbar";
import { AuthService } from './core/services/auth-service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { AdminSidebar } from './shared/admin-sidebar/admin-sidebar';
import { User } from './core/models/user';
import { filter, Subject } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, AsyncPipe, AdminSidebar, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected title = 'CustomerSupport-Client';
  user: User | null = null;

  constructor(public authService: AuthService, private route: Router) {
    this.route.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        authService.routeSubject.next(event.urlAfterRedirects);
      })
  }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe({
      next: (data) => this.user = data
    })
  }
}
