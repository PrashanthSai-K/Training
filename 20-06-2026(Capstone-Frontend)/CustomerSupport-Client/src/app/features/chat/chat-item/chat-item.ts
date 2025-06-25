import { Component, Input, OnInit } from '@angular/core';
import { ChatModel } from '../../../core/models/chat';
import { LucideAngularModule } from 'lucide-angular';
import { AsyncPipe, CommonModule, TitleCasePipe } from '@angular/common';
import { ChatService } from '../../../core/services/chat-service';
import { AuthService } from '../../../core/services/auth-service';

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

  constructor(private chatService: ChatService, public authService: AuthService) {
  }

  ngOnInit(): void {
    this.chatService.activeChat$.subscribe({
      next: (data) => {
        this.activeChat = data as ChatModel;
      }
    })
  }
}
