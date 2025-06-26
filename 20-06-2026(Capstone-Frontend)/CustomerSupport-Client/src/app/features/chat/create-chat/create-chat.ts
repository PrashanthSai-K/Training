import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, input, Output, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LucideAngularModule } from 'lucide-angular';
import { ChatService } from '../../../core/services/chat-service';
import { ChatForm, ChatModel } from '../../../core/models/chat';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-create-chat',
  imports: [CommonModule, LucideAngularModule, ReactiveFormsModule],
  templateUrl: './create-chat.html',
  styleUrl: './create-chat.css'
})
export class CreateChat {

  @Output() closeChatEmit = new EventEmitter<boolean>();
  isSubmitting = signal<boolean>(false);
  chatForm = new FormGroup({
    issueName: new FormControl("", [Validators.required, Validators.minLength(5), Validators.maxLength(20)]),
    issueDescription: new FormControl("", [Validators.required, Validators.minLength(20), Validators.maxLength(100)])
  });
  private _snackBar = inject(MatSnackBar);

  public get issueName(): any {
    return this.chatForm.get('issueName');
  }

  public get issueDescription(): any {
    return this.chatForm.get('issueDescription');
  }

  constructor(private chatService: ChatService) {
  }

  onSubmit() {
    if (this.chatForm.invalid)
      return
    this.isSubmitting.set(true);
    this.chatService.createChat(this.chatForm.value as ChatForm).subscribe({
      next: (data) => {
        console.log(data);
        this.isSubmitting.set(false);
        this._snackBar.open("Agent has been assigned.", "", {
          duration: 1000
        })
        this.chatService.getChats().subscribe();
        this.chatService.setActiveChat(data as ChatModel);
      },
      error: (err) => {
        this.isSubmitting.set(false);
        console.log(err);
      },
      complete: () => {
        this.closeChatEmit.emit(true);
      }
    })
  }

}
