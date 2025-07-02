import { Component, OnInit, signal } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Navbar } from "./shared/navbar/navbar";
import { AuthService } from './core/services/auth-service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { AdminSidebar } from './shared/admin-sidebar/admin-sidebar';
import { User } from './core/models/user';
import { filter, Subject } from 'rxjs';
import { Notification } from "./shared/notification/notification";
import { provideNoopAnimations } from '@angular/platform-browser/animations';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, AsyncPipe, AdminSidebar, CommonModule, Notification],
  // providers: [provideNoopAnimations()],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected title = 'CustomerSupport-Client';
  user: User | null = null;
  isNavOpen = signal(window.innerWidth > 640 ? true : false);
  isNotificationOpen = signal(false);
  isNavVisible = signal(false);

  constructor(public authService: AuthService, private route: Router) {
    this.route.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        authService.routeSubject.next(event.urlAfterRedirects);
      })
  }
  toggledNavbar(event: any) {
    console.log("toggled nav");
    this.isNavOpen.set(!this.isNavOpen());
  }

  toggleNotification(event: any) {
    this.isNotificationOpen.set(!this.isNotificationOpen());
  }

  url = ['login', 'register', 'forgotPassword', 'resetPassword?']

  navChanged(event: string) {
    if (event == "/") {
      return this.isNavVisible.set(false);
    }
    this.isNavVisible.set(!this.url.some(u => event.includes(u)))
  }
  ngOnInit(): void {
    this.authService.currentUser$.subscribe({
      next: (data) => this.user = data
    });
  }
}
