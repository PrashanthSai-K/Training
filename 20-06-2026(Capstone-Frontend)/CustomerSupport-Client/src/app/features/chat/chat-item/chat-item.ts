import { Component, Input, OnInit, signal } from '@angular/core';
import { ChatModel } from '../../../core/models/chat';
import { LucideAngularModule } from 'lucide-angular';
import { AsyncPipe, CommonModule, TitleCasePipe } from '@angular/common';
import { ChatService } from '../../../core/services/chat-service';
import { AuthService } from '../../../core/services/auth-service';
import { NotificationService } from '../../../core/services/notification-service';

@Component({
  selector: 'app-chat-item',
  imports: [LucideAngularModule, TitleCasePipe, CommonModule, AsyncPipe],
  templateUrl: './chat-item.html',
  styleUrl: './chat-item.css'
})
export class ChatItem implements OnInit {

  @Input() chat!: ChatModel;
  iconColor = "";
  activeChat: ChatModel | null = null;
  isActive: number | null = null;
  newMessages = signal(false);

  constructor(private chatService: ChatService, public authService: AuthService, private notificationService: NotificationService) {
  }

  ngOnInit(): void {
    this.chatService.activeChat$.subscribe({
      next: (data) => {
        this.activeChat = data as ChatModel;
        if (this.activeChat?.id == this.chat?.id) {
          this.newMessages.set(false);
          this.notificationService.removeNotifications(this.chat.id);
        }
      }
    });
    this.notificationService.notification$.subscribe({
      next: (data) => {
        data.map((d) => {
          if (d.chatId == this.chat.id && (!this.activeChat || this.activeChat.id != d.chatId) && !d.issueName) {
            this.newMessages.set(true);
          }
        })
      }
    })
  }
}
