import { Component, EventEmitter, Input, OnInit, Output, signal } from '@angular/core';
import { ChatService } from '../../../core/services/chat-service';
import { ChatModel } from '../../../core/models/chat';
import { ChatItem } from '../chat-item/chat-item';
import { Observable } from 'rxjs';
import { ChatHubService } from '../../../core/services/chat-hub-service';
import { Message } from '../../../core/models/message';
import { NotificationService } from '../../../core/services/notification-service';
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
  activeChat: ChatModel | null = null

  constructor(private chatService: ChatService, private authService: AuthService, private chatHubService: ChatHubService, private notificationService: NotificationService) {
  }

  setMessage(chat: ChatModel) {
    this.chatService.setActiveChat(chat);
    this.showChat.emit(true);
  }

  connectToHub(chats: ChatModel[]) {
    chats.map((chat) => {
      this.chatHubService.joinChat(chat.id);
    })
  }

  pushNotification(message: Message) {
    if (this.activeChat?.id == message.chatId) {
      return;
    }
    this.notificationService.addNewNotification(message);
  }

  ngOnInit(): void {
    this.isLoading.set(true);
    this.chatService.getChats().subscribe({
      next: (data) => {
        this.chats = data as ChatModel[];
        this.connectToHub(this.chats);
        this.isLoading.set(false);
      }
    });
    this.chatService.activeChat$.subscribe({
      next: (chat) => {
        this.activeChat = chat;
      }
    });

    this.chatService.chat$.subscribe({
      next: (chats) => {
        this.chats = chats;
        this.connectToHub(this.chats);
      }
    })
    this.chatHubService.messages$.subscribe({
      next: (message) => {
        if (message)
          this.pushNotification(message);
      }
    })
    this.authService.currentUser$.subscribe({
      next: (data) => {
        this.chatHubService.closedChat$.subscribe({
          next: (chat) => {
            this.chatService.getChats().subscribe();
            if (chat && (chat.agentEmail == data?.username || chat.customerEmail == data?.username)) {
              this.pushNotification(chat);
            }
          }
        })
      }
    })

    this.authService.currentUser$.subscribe({
      next: (data) => {
        this.chatHubService.assignedChat$.subscribe({
          next: (chat) => {
            this.chatService.getChats().subscribe();
            if (chat && (chat.agentEmail == data?.username || chat.customerEmail == data?.username))
              this.pushNotification(chat);
          }
        })

      }
    })
  }
}
