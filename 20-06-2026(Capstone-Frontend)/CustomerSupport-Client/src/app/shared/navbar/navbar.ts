import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';
import { LucideAngularModule, BotMessageSquare } from 'lucide-angular';
import { every, filter } from 'rxjs';
import { AuthService } from '../../core/services/auth-service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { User } from '../../core/models/user';

@Component({
  selector: 'app-navbar',
  imports: [LucideAngularModule, RouterLink, AsyncPipe, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {

  isVisible: boolean = true;
  user: User | null = null;
  constructor(private route: Router, public authService: AuthService) {
    this.route.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const url = event.urlAfterRedirects;
        this.isVisible = !(url == "/login" || url == "/register" || url == "/");
      })
      authService.currentUser$.subscribe({
        next:(data)=>{
          this.user = data;
        }
      })
  }

  onLogout() {
    this.authService.logoutUser();
  }
}
