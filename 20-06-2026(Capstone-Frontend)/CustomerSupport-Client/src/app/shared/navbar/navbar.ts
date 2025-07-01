import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';
import { LucideAngularModule, BotMessageSquare } from 'lucide-angular';
import { every, filter, Observable } from 'rxjs';
import { AuthService } from '../../core/services/auth-service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { User } from '../../core/models/user';
import { Subject } from '@microsoft/signalr';
import { NotificationService } from '../../core/services/notification-service';

@Component({
  selector: 'app-navbar',
  imports: [LucideAngularModule, RouterLink, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements OnInit {

  isVisible: boolean = true;
  user: User | null = null;
  notificationAvailable = signal(false);
  @Output() toggledNavbar = new EventEmitter();
  @Output() toggledNotification = new EventEmitter();
  @Output() navDisabled = new EventEmitter<string>();
  // @Input() route$ !: Observable<string>;

  constructor(private route: Router, public authService: AuthService, public notificationService: NotificationService) {
    authService.currentUser$.subscribe({
      next: (data) => {
        this.user = data;
      }
    })
  }
  toggleNavbar() {
    this.toggledNavbar.emit();
  }
  toggleNotification() {
    this.toggledNotification.emit();
  }
  url = ['login', 'register', 'forgotPassword', 'resetPassword?']
  ngOnInit(): void {
    this.authService.route$.subscribe({
      next: (url) => {
        this.navDisabled.emit(url);
        if (url == "/")
          this.isVisible = false;
        else
          this.isVisible = !this.url.some(u => url.includes(u));
      }
    });
    this.notificationService.notification$.subscribe({
      next: (data) => {
        if (data?.length > 0)
          this.notificationAvailable.set(true);
        else
          this.notificationAvailable.set(false);
      }
    })
  }

  onLogout() {
    this.authService.logoutUser();
  }
}
