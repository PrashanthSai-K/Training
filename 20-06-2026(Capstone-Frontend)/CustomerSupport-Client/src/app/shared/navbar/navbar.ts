import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';
import { LucideAngularModule, BotMessageSquare } from 'lucide-angular';
import { every, filter } from 'rxjs';
import { AuthService } from '../../core/services/auth-service';
import { AsyncPipe, CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  imports: [LucideAngularModule, RouterLink, AsyncPipe, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {

  isVisible:boolean = true;
  constructor(private route:Router, public authService:AuthService){
    this.route.events
        .pipe(filter(event=>event instanceof NavigationEnd))
        .subscribe((event:NavigationEnd)=>{
          const url = event.urlAfterRedirects;
          this.isVisible = !(url == "/login" || url == "/register" || url == "/");
        })
  }

  onLogout(){
    this.authService.logoutUser();
  }
}
