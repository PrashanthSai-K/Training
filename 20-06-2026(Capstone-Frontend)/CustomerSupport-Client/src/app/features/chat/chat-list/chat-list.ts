import { Component, EventEmitter, Input, OnInit, Output, signal } from '@angular/core';
import { ChatService } from '../../../core/services/chat-service';
import { ChatModel } from '../../../core/models/chat';
import { ChatItem } from '../chat-item/chat-item';
import { Observable } from 'rxjs';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-chat-list',
  imports: [ChatItem],
  templateUrl: './chat-list.html',
  styleUrl: './chat-list.css'
})
export class ChatList implements OnInit {

  chats: ChatModel[] | [] = [];
  @Input() scrolledBottom!: Observable<boolean>;
  @Output() showChat = new EventEmitter<boolean>();
  isLoading = signal<boolean>(true);

  constructor(private chatService: ChatService) {
  }

  setMessage(chat: ChatModel) {
    this.chatService.setActiveChat(chat);
    this.showChat.emit(true);
  }

  ngOnInit(): void {
    this.isLoading.set(true);
    this.chatService.getChats().subscribe({
      next: (data) => {
        this.chats = data as ChatModel[];
        this.isLoading.set(false);
      }
    })

    this.scrolledBottom.subscribe({
      next: (data) => {
        if (data) {
          this.chatService.pageNumber += 1
          this.chatService.getChats();
        }
      }
    })
  }

}
