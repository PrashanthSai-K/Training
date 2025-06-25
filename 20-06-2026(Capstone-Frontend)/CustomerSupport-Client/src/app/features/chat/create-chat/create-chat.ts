import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-create-chat',
  imports: [CommonModule, LucideAngularModule],
  templateUrl: './create-chat.html',
  styleUrl: './create-chat.css'
})
export class CreateChat {

}
