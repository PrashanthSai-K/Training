import { Component, inject, Input, OnInit } from '@angular/core';
import { ChatModel } from '../../../core/models/chat';
import { Observable } from 'rxjs';
import { ChatMessageService } from '../../../core/services/chat-message-service';
import { Message } from '../../../core/models/message';

@Component({
  selector: 'app-chat',
  imports: [],
  templateUrl: './chat.html',
  styleUrl: './chat.css'
})
export class Chat implements OnInit{
  @Input() chatData!: Observable<ChatModel>
  private chatMessageService = inject(ChatMessageService);
  chatMessages : any = {};

  ngOnInit(): void {
    this.chatData.subscribe({
      next : (data)=> this.chatMessageService.getChatMessages(data.id).subscribe({
        next : (message) => {
          this.chatMessages = message;
          console.log(this.chatMessages);
        }
      })
    })
  }

  

}
