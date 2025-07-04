import { Component, Input, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { AuthService } from '../../core/services/auth-service';
import { LucideAngularModule } from 'lucide-angular';
import { NavigationEnd, Router } from '@angular/router';
import { filter, Observable } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-sidebar',
  imports: [LucideAngularModule, CommonModule],
  templateUrl: './admin-sidebar.html',
  styleUrl: './admin-sidebar.css'
})
export class AdminSidebar {
  url = signal<string | null>(null);
  @Input()isNavOpen!:boolean;

  constructor(public authservice: AuthService) {
    authservice.route$.subscribe({
      next: (data) => {
        this.url.set(data);
      }
    })
  }

}
