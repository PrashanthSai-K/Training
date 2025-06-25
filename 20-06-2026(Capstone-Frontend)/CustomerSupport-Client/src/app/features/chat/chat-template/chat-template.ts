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
import { CreateChat } from "../create-chat/create-chat";

@Component({
  selector: 'app-chat-template',
  imports: [ChatList, Chat, LucideAngularModule, TitleCasePipe, AsyncPipe, CommonModule, CreateChat],
  templateUrl: './chat-template.html',
  styleUrl: './chat-template.css'
})
export class ChatTemplate implements OnInit {

  scrolledBottom = new BehaviorSubject<boolean>(false);
  scrolledBottom$ = this.scrolledBottom.asObservable();
  activeChat: ChatModel | null = null;
  user: User | null = null;
  isMobile: boolean = false;
  isListActive = signal<boolean>(true);
  isChatActive = signal<boolean>(false);
  isCreateChatActive = signal<boolean>(false);

  constructor(private chatService: ChatService, public authService: AuthService) {
  }

  ngOnInit(): void {
    console.log(this.showChat);

    this.isMobile = window.innerWidth < 640;
    window.addEventListener('resize', () => {
      this.isMobile = window.innerWidth < 640;
    })
    this.chatService.activeChat$.subscribe({
      next: (data) => {
        this.activeChat = data as ChatModel;
      }
    });
  }

  showCreateChat() {
    this.isCreateChatActive.set(true);
  }

  closeCreateChat() {
    this.isCreateChatActive.set(false);
  }

  //Mobile view chat controls
  showChat(event: boolean) {
    this.isListActive.set(false);
    this.isChatActive.set(true);
    console.log(this.isListActive());
  }

  backToList() {
    this.isListActive.set(true);
    this.isChatActive.set(false);
  }

  //Infinite scroller for message container
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
