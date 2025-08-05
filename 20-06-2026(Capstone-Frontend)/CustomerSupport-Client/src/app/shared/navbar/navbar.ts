import { Component, ElementRef, EventEmitter, HostListener, inject, Input, OnInit, Output, signal, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';
import { LucideAngularModule, BotMessageSquare } from 'lucide-angular';
import { every, filter, Observable } from 'rxjs';
import { AuthService } from '../../core/services/auth-service';
import { AsyncPipe, CommonModule, TitleCasePipe } from '@angular/common';
import { User } from '../../core/models/user';
import { Subject } from '@microsoft/signalr';
import { NotificationService } from '../../core/services/notification-service';
import { AgentService } from '../../core/services/agent-service';

@Component({
  selector: 'app-navbar',
  imports: [LucideAngularModule, CommonModule, TitleCasePipe],
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
  agent: any;
  isNavOpen = signal(window.innerWidth > 640 ? true : false);

  constructor(private route: Router, public authService: AuthService, public notificationService: NotificationService, public agentService: AgentService) {
    authService.currentUser$.subscribe({
      next: (data) => {
        this.user = data;
        if (this.user?.role == "Agent") {
          agentService.getAgent().subscribe({
            next: (data) => {
              this.agent = data;
            }
          });
        }
      }
    })
  }
  @ViewChild('dropdown') dropdownRef!: ElementRef;

  showDropdown = signal(false);

  toggleDropdown() {
    this.showDropdown.set(!this.showDropdown());
  }

  updateStatus(status: string) {
    this.agent.status = status;
    this.showDropdown.set(false);
    if (status == "Active") {
      this.agentService.activateAgent(this.agent.id).subscribe({
        next: (data) => {
          this.showDropdown.set(false);
        }
      })
    }
    if (status = "Inactive") {
      this.agentService.deactivateAgent(this.agent.id).subscribe({
        next: (data) => {
          this.showDropdown.set(false);
        }
      })
    }
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: MouseEvent) {
    if (this.dropdownRef && !this.dropdownRef.nativeElement.contains(event.target)) {
      this.showDropdown.set(false);
    }
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
