import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Chat } from './chat';
import { ChatModel } from '../../../core/models/chat';
import { Message } from '../../../core/models/message';
import { BehaviorSubject, of } from 'rxjs';
import { ChatService } from '../../../core/services/chat-service';
import { ChatHubService } from '../../../core/services/chat-hub-service';
import { AuthService } from '../../../core/services/auth-service';
import { ChangeDetectorRef, importProvidersFrom } from '@angular/core';
import { LucideAngularModule, SendHorizontal } from 'lucide-angular';

describe('Chat', () => {
  let component: Chat;
  let fixture: ComponentFixture<Chat>;

  let chatServiceMock: any;
  let chatHubServiceMock: any;
  let authServiceMock: any;
  let cdrMock: any;

  beforeEach(async () => {
    chatServiceMock = {
      activeChat$: new BehaviorSubject<ChatModel | null>({
        id: 1,
        issueName: 'Test Issue',
        issueDescription: '',
        status: 'Active',
        AgentId: 1,
        CustomerId: 1,
        createdOn: '',
        agent: { id: 1, name: 'Agent', email: '', status: '', dateOfJoin: '' },
        customer: { id: 1, name: 'Customer', email: '', status: '', phone: '' },
        updatedAt:""
      }),
      getChatMessages: jasmine.createSpy().and.returnValue(of([])),
      getChats: jasmine.createSpy().and.returnValue(of([])),
      sendTextMessage: jasmine.createSpy().and.returnValue(of({})),
      sendImage: jasmine.createSpy().and.returnValue(of({})),
      getChatImage: jasmine.createSpy().and.returnValue(of({ imageUrl: 'mock-url' })),
      appendMessages: jasmine.createSpy()
    };

    chatHubServiceMock = {
      joinChat: jasmine.createSpy('joinChat'),
      messages$: new BehaviorSubject<Message | null>(null)
    };

    authServiceMock = {
      currentUser$: new BehaviorSubject({ username: 'Agent', role: 'Agent' })
    };

    cdrMock = {
      detectChanges: jasmine.createSpy('detectChanges')
    };

    await TestBed.configureTestingModule({
      imports: [Chat],
      providers: [
        { provide: ChatService, useValue: chatServiceMock },
        { provide: ChatHubService, useValue: chatHubServiceMock },
        { provide: AuthService, useValue: authServiceMock },
        { provide: ChangeDetectorRef, useValue: cdrMock },
        importProvidersFrom(LucideAngularModule.pick({SendHorizontal}))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Chat);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create Chat component', () => {
    expect(component).toBeTruthy();
  });

  it('should send a text message and clear input', () => {
    component.message = 'Hello';
    component.chat = chatServiceMock.activeChat$.value;
    component.onSendMessage();
    expect(chatServiceMock.sendTextMessage).toHaveBeenCalledWith(1, 'Hello');
  });

  it('should send image if previewVisible is true and file is set', () => {
    component.chat = chatServiceMock.activeChat$.value;
    component.previewVisible.set(true);
    component.file = new File(['dummy'], 'image.jpg', { type: 'image/jpeg' });

    component.onSendMessage();

    expect(chatServiceMock.sendImage).toHaveBeenCalled();
    expect(component.previewImageUrl).toBe('');
    expect(component.previewVisible()).toBeFalse();
    expect(component.file).toBeNull();
  });

  it('should load chat messages and join chat on init', () => {
    expect(chatHubServiceMock.joinChat).toHaveBeenCalledWith(1);
    expect(chatServiceMock.getChatMessages).toHaveBeenCalledWith(1);
  });

  it('should scrollToBottom when receiving a new message for active chat', () => {
    const spy = spyOn(component, 'scrollToBottom');
    component.chat = chatServiceMock.activeChat$.value;
    const msg: Message = {
      id: 1,
      chatId: 1,
      userId: 'Customer',
      message: 'Test msg',
      createdAt: '2025-06-01'
    };
    chatHubServiceMock.messages$.next(msg);
    expect(component.chatMessages.some(m => m.message === 'Test msg')).toBeTrue();
    expect(spy).toHaveBeenCalled();
  });
});
