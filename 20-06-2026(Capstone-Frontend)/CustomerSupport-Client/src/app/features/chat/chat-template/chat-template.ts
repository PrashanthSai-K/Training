import { ChangeDetectorRef, Component, effect, OnInit, signal } from '@angular/core';
import { ChatList } from "../chat-list/chat-list";
import { Chat } from "../chat/chat";
import { BehaviorSubject, Subject } from 'rxjs';
import { ChatModel } from '../../../core/models/chat';
import { LucideAngularModule } from 'lucide-angular';
import { ChatService } from '../../../core/services/chat-service';
import { AsyncPipe, CommonModule, TitleCasePipe } from '@angular/common';
import { AuthService } from '../../../core/services/auth-service';
import { User } from '../../../core/models/user';

@Component({
  selector: 'app-chat-template',
  imports: [ChatList, Chat, LucideAngularModule, TitleCasePipe, AsyncPipe, CommonModule],
  templateUrl: './chat-template.html',
  styleUrl: './chat-template.css'
})
export class ChatTemplate implements OnInit {

  scrolledBottom = new BehaviorSubject<boolean>(false);
  scrolledBottom$ = this.scrolledBottom.asObservable();
  activeChat: ChatModel | null = null;
  user: User | null = null;

  constructor(private chatService: ChatService, public authService: AuthService) {
  }

  ngOnInit(): void {
    this.chatService.activeChat$.subscribe({
      next: (data) => {
        this.activeChat = data as ChatModel;
      }
    });

    // this.authService.currentUser$.subscribe({
    //   next: (data) => {
    //     this.user = data;
    //     console.log(this.user);
    //   }
    // })
  }

  onScroll(event: Event) {
    const element = event.target as HTMLElement;
    const scrollTop = element.scrollTop;
    const scrollHeight = element.scrollHeight;
    const clientHeight = element.clientHeight;

    const scrolledToBottom = scrollTop + clientHeight >= scrollHeight - 100;

    if (scrolledToBottom) {
      this.scrolledBottom.next(true);
    }
  }
}
