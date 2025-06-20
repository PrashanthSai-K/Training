import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { LucideAngularModule, BotMessageSquare } from 'lucide-angular';
import { every, filter } from 'rxjs';

@Component({
  selector: 'app-navbar',
  imports: [LucideAngularModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {

  isVisible:boolean = true;
  constructor(private route:Router){
    this.route.events
        .pipe(filter(event=>event instanceof NavigationEnd))
        .subscribe((event:NavigationEnd)=>{
          const url = event.urlAfterRedirects;
          console.log(url);
          this.isVisible = !(url == "/login" || url == "/register");
        })
  }
}
