import { ChangeDetectorRef, Component, effect, inject, OnInit, signal } from '@angular/core';
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
import { FormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-chat-template',
  imports: [ChatList, Chat, LucideAngularModule, TitleCasePipe, AsyncPipe, CommonModule, CreateChat, FormsModule],
  templateUrl: './chat-template.html',
  styleUrl: './chat-template.css'
})
export class ChatTemplate implements OnInit {

  activeChat: ChatModel | null = null;
  user: User | null = null;
  isMobile: boolean = false;
  isListActive = signal<boolean>(true);
  isChatActive = signal<boolean>(false);
  isCreateChatActive = signal<boolean>(false);
  isPreviewVisible = signal<boolean>(false);
  searchQuery: string = "";
  filterQuery: string = "";
  previewImage: string = "";

  private _snackBar = inject(MatSnackBar);

  constructor(public chatService: ChatService, public authService: AuthService) {
  }

  ngOnInit(): void {
    this.isMobile = window.innerWidth < 640;
    window.addEventListener('resize', () => {
      this.isMobile = window.innerWidth < 640;
    })
    this.chatService.activeChat$.subscribe({
      next: (data) => {
        this.activeChat = data as ChatModel;
      }
    });
    this.chatService.previewImage$.subscribe({
      next: (data) => {
        if (data) {
          this.previewImage = data;
          this.isPreviewVisible.set(true);
          console.log(data);
        }
      }
    })
  }

  onCloseChat() {
    try {
      if (!window.confirm("Do you want to mark the issue as closed ?")) {
        return
      }
      if (this.activeChat)
        this.chatService.closeChat(this.activeChat.id).subscribe({
          next: (data) => {
            this._snackBar.open("Issue has been closed", "", {
              duration: 10000
            })
            if (this.activeChat)
              this.activeChat.status = 'Deleted';
          },
          error: (err) => {
            console.log(err);
          }
        })
    } catch (error) {
      console.log(error);
    }
  }

  closePreview() {
    this.isPreviewVisible.set(false);
    this.previewImage = "";
  }

  onSearch() {
    this.chatService.searchSubject.next(this.searchQuery);
  }

  onAllFilter() {
    this.filterQuery = '';
    this.chatService.filterSubject.next(this.filterQuery);
  }

  onActiveFilter() {
    this.filterQuery = 'active';
    this.chatService.filterSubject.next(this.filterQuery);
  }

  onClosedFilter() {
    this.filterQuery = 'deleted';
    this.chatService.filterSubject.next(this.filterQuery);
  }

  showCreateChat() {
    this.isCreateChatActive.set(true);
  }

  closeCreateChat(event: boolean) {
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
}
