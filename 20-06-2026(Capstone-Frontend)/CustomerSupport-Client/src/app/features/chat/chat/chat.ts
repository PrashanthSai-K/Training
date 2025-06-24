import { ChangeDetectorRef, Component, ElementRef, inject, Input, OnInit, ViewChild } from '@angular/core';
import { ChatModel } from '../../../core/models/chat';
import { Message } from '../../../core/models/message';
import { CommonModule } from '@angular/common';
import { ChatService } from '../../../core/services/chat-service';
import { LucideAngularModule } from 'lucide-angular';
import { FormsModule } from '@angular/forms';
import { ChatHubService } from '../../../core/services/chat-hub-service';


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

  constructor(private chatService: ChatService, private chatHubService: ChatHubService, private cdr: ChangeDetectorRef) {
  }

  onSendMessage() {
    console.log(this.message);
    if (this.chat?.id) {
      this.chatService.sendTextMessage(this.chat.id, this.message).subscribe({
        next: (data:any) => {
          console.log(data);
          this.message = "";
          this.scrollToBottom();
        }
      })
    }
  }

  @ViewChild('scrollContainer') scrollContainer!: ElementRef<HTMLDivElement>;

  scrollToBottom() {
    this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
  }

  ngOnInit(): void {
    this.chatService.activeChat$.subscribe({
      next: (data) => {
        if (data) {
          this.chat = data as ChatModel;
          this.chatService.getChatMessages(data.id);
          this.chatHubService.startConnection(data.id)
        }
      }
    });

    this.chatService.messages$.subscribe({
      next: (data) => {
        this.chatMessages = data as Message[];
        this.cdr.detectChanges();
        this.scrollToBottom();
      }
    });

    this.chatHubService.messages$.subscribe({
      next: (data) => {
        this.chatMessages = [...this.chatMessages, data as Message];
        this.cdr.detectChanges();
        this.scrollToBottom();
      }
    })
  }
}
