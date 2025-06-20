import { AfterViewInit, Component, ElementRef, HostListener, Input, OnInit, Signal, viewChild } from '@angular/core';
import { ChatService } from '../../../core/services/chat-service';
import { ChatModel } from '../../../core/models/chat';
import { ChatItem } from '../chat-item/chat-item';
import { Observable } from 'rxjs';
import { Chat } from '../chat/chat';
import { ChatTemplate } from '../chat-template/chat-template';

@Component({
  selector: 'app-chat-list',
  imports: [ChatItem],
  templateUrl: './chat-list.html',
  styleUrl: './chat-list.css'
})
export class ChatList implements OnInit {

  chats: ChatModel[] | [] = [];
  
  pageSize: number = 10;
  pageNumber: number = 1;
  @Input() scrolledBottom!: Observable<boolean>;

  constructor(private chatService: ChatService,private chatTemplate : ChatTemplate) {

  }

  setMessage(message : ChatModel){
    this.chatTemplate.setMessage(message)
  }

  ngOnInit(): void {
    this.chatService.getChats(this.pageNumber, this.pageSize).subscribe({
      next: (data) => {
        this.chats = data as ChatModel[];
        console.log(data);
      }
    })

    this.scrolledBottom.subscribe({
      next: (data) => {
        if (data) {
          this.chatService.getChats(this.pageNumber, this.pageSize).subscribe({
            next: (data) => {
              this.pageNumber += 1;
              this.chats = [...this.chats, ...data as ChatModel[]];
              console.log(data);
            }
          })
        }
      }
    })
  }

}
