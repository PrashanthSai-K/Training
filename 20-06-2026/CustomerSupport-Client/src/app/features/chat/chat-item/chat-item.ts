import { Component, Input } from '@angular/core';
import { ChatModel } from '../../../core/models/chat';
import { LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-chat-item',
  imports: [LucideAngularModule],
  templateUrl: './chat-item.html',
  styleUrl: './chat-item.css'
})
export class ChatItem {
  @Input() chat!:ChatModel;
  
}
