import { ChangeDetectorRef, Component, ElementRef, inject, Input, OnInit, signal, ViewChild } from '@angular/core';
import { ChatModel } from '../../../core/models/chat';
import { Message } from '../../../core/models/message';
import { CommonModule } from '@angular/common';
import { ChatService } from '../../../core/services/chat-service';
import { LucideAngularModule } from 'lucide-angular';
import { FormsModule } from '@angular/forms';
import { ChatHubService } from '../../../core/services/chat-hub-service';
import { AuthService } from '../../../core/services/auth-service';
import { filter, switchMap } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-chat',
  imports: [CommonModule, LucideAngularModule, FormsModule],
  templateUrl: './chat.html',
  styleUrl: './chat.css'
})
export class Chat implements OnInit {

  chat: ChatModel | null = null;
  chatMessages: Message[] = [];
  message: string = "";
  images: { [key: string]: string } = {};
  previewImageUrl: string = "";
  previewVisible = signal<boolean>(false);
  file: File | null = null;

  private _snackBar = inject(MatSnackBar);

  constructor(private chatService: ChatService, private chatHubService: ChatHubService, private cdr: ChangeDetectorRef, public authService: AuthService) {
  }

  onSendMessage() {
    if (this.chat?.id) {
      if (this.previewVisible() && this.file != null) {
        console.log("triggered me");
        this.chatService.sendImage(this.chat.id, this.file).subscribe({
          next: () => {
            this.previewImageUrl = "";
            this.previewVisible.set(false);
            this.file = null;
          },
          complete: () => {
            this.scrollToBottom();
            this.chatService.getChats();
          }
        })
      }

      if (this.message.trim() != "")
        this.chatService.sendTextMessage(this.chat.id, this.message).subscribe({
          next: (data: any) => {
            console.log(data);
            this.message = "";
          },
          complete: () => {
            this.scrollToBottom();
            this.chatService.getChats();
          }
        });
    }
  }

  onSubmit() {

  }

  @ViewChild('scrollContainer') scrollContainer!: ElementRef<HTMLDivElement>;

  scrollToBottom() {
    this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    input.files && console.log(input?.files[0].name, input.files[0].type);
    if (input.files && input.files[0].size >= 1 * 1024 * 1024) {
      return this._snackBar.open("File size must be less than 1 mb", "", {
        duration: 2000
      })
    }
    const reader = new FileReader();
    if (input.files) {
      reader.readAsBinaryString(input.files[0]);
      this.file = input.files[0];
      reader.onload = (eve) => {
        this.previewImageUrl = btoa(eve.target?.result?.toString() || "");
        this.previewVisible.set(true);
      };
    }
    return
  }

  onPreviewClose() {
    this.previewVisible.set(false);
    this.previewImageUrl = "";
    this.file = null
  }

  getImage(name: string): string {
    if (this.chat) {
      if (!this.images[name]) {
        this.chatService.getChatImage(this.chat.id, name).subscribe({
          next: (data: any) => {
            this.images[name] = data.imageUrl;
          }
        })
      }
      return this.images[name];
    }
    return "";
  }

  previewImage(name: string) {
    this.chatService.previewImageSubject.next(this.getImage(name));
  }

  ngOnInit(): void {
    this.chatService.activeChat$.pipe(
      filter((chat): chat is ChatModel => !!chat),
      switchMap(chat => {
        this.chat = chat as ChatModel;
        this.chatHubService.joinChat(chat.id);
        return this.chatService.getChatMessages(chat.id);
      })
    ).subscribe({
      next: (messages) => {
        this.chatMessages = messages as Message[];
        this.cdr.detectChanges();
        this.scrollToBottom();
      }
    });
    this.chatHubService.messages$.subscribe({
      next: (data) => {
        if (data) {
          if (data.chatId == this.chat?.id) {
            this.chatMessages.push(data);
            this.cdr.detectChanges();
            this.scrollToBottom();
          }
        }
      }
    })
    this.authService.currentUser$.subscribe({
      next: (data) => {
        this.chatHubService.closedChat$.subscribe({
          next: (notification) => {
            this.chatService.getChats().subscribe();
            if (this.chat && notification && notification.chatId == this.chat.id) {
              this.chat.status = 'Deleted';
            }
          }
        })
      }
    })
  }
}
