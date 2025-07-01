import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateChat } from './create-chat';
import { ChatService } from '../../../core/services/chat-service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { of, throwError } from 'rxjs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChatModel } from '../../../core/models/chat';
import { importProvidersFrom } from '@angular/core';
import { LucideAngularModule, MessageSquarePlus } from 'lucide-angular';

describe('CreateChat', () => {
  let component: CreateChat;
  let fixture: ComponentFixture<CreateChat>;

  let chatServiceMock: any;
  let snackBarMock: any;

  beforeEach(async () => {
    chatServiceMock = {
      createChat: jasmine.createSpy('createChat').and.returnValue(of({ id: 1 } as ChatModel)),
      getChats: jasmine.createSpy('getChats').and.returnValue(of([])),
      setActiveChat: jasmine.createSpy('setActiveChat')
    };


    snackBarMock = {
      open: jasmine.createSpy('open')
    };

    await TestBed.configureTestingModule({
      imports: [CreateChat, ReactiveFormsModule],
      providers: [
        { provide: ChatService, useValue: chatServiceMock },
        { provide: MatSnackBar, useValue: snackBarMock },
        importProvidersFrom(LucideAngularModule.pick({ MessageSquarePlus }))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateChat);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should disable submission if form is invalid', () => {
    component.chatForm.setValue({ issueName: '', issueDescription: '' });
    component.onSubmit();
    expect(chatServiceMock.createChat).not.toHaveBeenCalled();
  });

  it('should submit form if valid and trigger success flow', () => {
    spyOn(component.closeChatEmit, 'emit');
    component.chatForm.setValue({
      issueName: 'Login Issue',
      issueDescription: 'User cannot log in using correct credentials'
    });
    component.onSubmit();

    expect(chatServiceMock.createChat).toHaveBeenCalled();
    expect(snackBarMock.open).toHaveBeenCalledWith("Agent has been assigned.", "", { duration: 1000 });
    expect(chatServiceMock.setActiveChat).toHaveBeenCalledWith({ id: 1 });
    expect(component.closeChatEmit.emit).toHaveBeenCalledWith(true);
  });

  it('should handle errors and not emit or notify', () => {
    chatServiceMock.createChat.and.returnValue(throwError(() => new Error('Server error')));
    spyOn(component.closeChatEmit, 'emit');
    component.chatForm.setValue({
      issueName: 'Login Error',
      issueDescription: 'User fails to login repeatedly'
    });

    component.onSubmit();

    expect(snackBarMock.open).not.toHaveBeenCalled();
    expect(component.closeChatEmit.emit).not.toHaveBeenCalled();
    expect(component.isSubmitting()).toBeFalse();
  });
});
