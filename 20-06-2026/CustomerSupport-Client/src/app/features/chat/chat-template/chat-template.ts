import { Component } from '@angular/core';
import { ChatList } from "../chat-list/chat-list";
import { Chat } from "../chat/chat";
import { BehaviorSubject, Subject } from 'rxjs';
import { ChatModel } from '../../../core/models/chat';

@Component({
  selector: 'app-chat-template',
  imports: [ChatList, Chat],
  templateUrl: './chat-template.html',
  styleUrl: './chat-template.css'
})
export class ChatTemplate {

  scrolledBottom = new BehaviorSubject<boolean>(false);
  scrolledBottom$ = this.scrolledBottom.asObservable();
  private chatData = new Subject<ChatModel>();
  chatData$ = this.chatData.asObservable();


  setMessage(message : ChatModel){
    this.chatData.next(message);
  }

  onScroll(event: Event) {
    console.log("scrolled");

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
