import { AfterViewInit, Component, effect, ElementRef, EventEmitter, HostListener, Input, OnChanges, OnInit, Output, signal, Signal, SimpleChanges, viewChild, WritableSignal } from '@angular/core';
import { ChatService } from '../../../core/services/chat-service';
import { ChatModel } from '../../../core/models/chat';
import { ChatItem } from '../chat-item/chat-item';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { Chat } from '../chat/chat';
import { ChatTemplate } from '../chat-template/chat-template';
import { LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-chat-list',
  imports: [ChatItem],
  templateUrl: './chat-list.html',
  styleUrl: './chat-list.css'
})
export class ChatList implements OnInit {

  chats: ChatModel[] | [] = [];
  @Input() scrolledBottom!: Observable<boolean>;

  constructor(private chatService: ChatService) {
  }

  setMessage(chat: ChatModel) {
    this.chatService.setActiveChat(chat);
  }

  ngOnInit(): void {

    this.chatService.getChats();
    this.chatService.chat$.subscribe({
      next: (data) => {
        this.chats = data as ChatModel[];
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
