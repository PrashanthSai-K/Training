import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, signal } from '@angular/core';
import { NotificationService } from '../../core/services/notification-service';
import { Message } from '../../core/models/message';

@Component({
  selector: 'app-notification',
  imports: [CommonModule],
  templateUrl: './notification.html',
  styleUrl: './notification.css'
})
export class Notification implements OnInit {

  @Input() isVisible!: boolean;
  notifications: Message[] = [];

  constructor(private notificationService: NotificationService) {

  }

  ngOnInit(): void {
    this.notificationService.notification$.subscribe({
      next: (data) => {
        console.log(data);
        
        this.notifications = data;
      }
    })
  }
}
