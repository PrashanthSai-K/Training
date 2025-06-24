import { Component, computed, effect, Input, OnChanges, OnInit, signal, SimpleChanges, WritableSignal } from '@angular/core';
import { ChatModel } from '../../../core/models/chat';
import { LucideAngularModule } from 'lucide-angular';
import { CommonModule, TitleCasePipe } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';
import { ChatService } from '../../../core/services/chat-service';

@Component({
  selector: 'app-chat-item',
  imports: [LucideAngularModule, TitleCasePipe, CommonModule],
  templateUrl: './chat-item.html',
  styleUrl: './chat-item.css'
})
export class ChatItem implements OnInit {

  @Input() chat!: ChatModel;
  iconColor = "";
  activeChat: ChatModel | null = null;
  isActive: number | null = null;

  constructor(private chatService: ChatService) {
  }

  ngOnInit(): void {
    this.chatService.activeChat$.subscribe({
      next: (data) => {
        this.activeChat = data as ChatModel;
      }
    })
  }
}
