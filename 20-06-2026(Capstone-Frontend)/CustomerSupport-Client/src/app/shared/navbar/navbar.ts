import { Component, inject, Input, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';
import { LucideAngularModule, BotMessageSquare } from 'lucide-angular';
import { every, filter, Observable } from 'rxjs';
import { AuthService } from '../../core/services/auth-service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { User } from '../../core/models/user';
import { Subject } from '@microsoft/signalr';

@Component({
  selector: 'app-navbar',
  imports: [LucideAngularModule, RouterLink, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements OnInit {

  isVisible: boolean = true;
  user: User | null = null;
  // @Input() route$ !: Observable<string>;

  constructor(private route: Router, public authService: AuthService) {
    authService.currentUser$.subscribe({
      next: (data) => {
        this.user = data;
      }
    })
  }
  url = ['login', 'register', 'forgotPassword', 'resetPassword?']
  ngOnInit(): void {
    this.authService.route$.subscribe({
      next: (url) => {
        if (url == "/")
          this.isVisible = false;
        else
          this.isVisible = !this.url.some(u => url.includes(u));
      }
    })
  }

  onLogout() {
    this.authService.logoutUser();
  }
}
