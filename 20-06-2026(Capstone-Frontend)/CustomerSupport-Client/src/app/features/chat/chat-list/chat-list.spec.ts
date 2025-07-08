import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChatList } from './chat-list';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { ChatService } from '../../../core/services/chat-service';
import { ChatHubService } from '../../../core/services/chat-hub-service';
import { NotificationService } from '../../../core/services/notification-service';
import { ChatModel } from '../../../core/models/chat';
import { Message } from '../../../core/models/message';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-chat-item',
  template: ''
})
class MockChatItem {
  @Input() chat!: ChatModel;
  @Output() click = new EventEmitter<void>();
}

describe('ChatList', () => {
  let component: ChatList;
  let fixture: ComponentFixture<ChatList>;

  let chatServiceMock: any;
  let chatHubServiceMock: any;
  let notificationServiceMock: any;
  let authServiceMock: any;


  beforeEach(async () => {
    // Mock services
    chatServiceMock = {
      getChats: jasmine.createSpy('getChats').and.returnValue(of([])),
      activeChat$: new BehaviorSubject<ChatModel | null>(null),
      chat$: new BehaviorSubject<ChatModel[]>([]),
      setActiveChat: jasmine.createSpy('setActiveChat'),
    };

    chatHubServiceMock = {
      joinChat: jasmine.createSpy('joinChat'),
      messages$: new BehaviorSubject<Message | null>(null),
      closedChat$: new BehaviorSubject<any>(null),
      assignedChat$: new BehaviorSubject<any>(null),
    };

    notificationServiceMock = {
      addNewNotification: jasmine.createSpy('addNewNotification'),
      notification$: new BehaviorSubject<Message[]>([])
    };

    authServiceMock = {
      currentUser$: new BehaviorSubject<any>({ username: 'john@example.com' })
    };

    await TestBed.configureTestingModule({
      imports: [ChatList, MockChatItem],
      // declarations: [],
      providers: [
        { provide: ChatService, useValue: chatServiceMock },
        { provide: ChatHubService, useValue: chatHubServiceMock },
        { provide: NotificationService, useValue: notificationServiceMock },
        { provide: AuthService, useValue: authServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ChatList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create ChatList component', () => {
    expect(component).toBeTruthy();
  });

  it('should call getChats and populate chats', () => {
    expect(chatServiceMock.getChats).toHaveBeenCalled();
    expect(component.chats).toEqual([]);
  });

  it('should call setMessage and emit showChat', () => {
    spyOn(component.showChat, 'emit');
    const chat: ChatModel = {
      id: 1,
      issueName: 'Issue',
      issueDescription: '',
      status: 'Active',
      AgentId: 1,
      CustomerId: 1,
      createdOn: '',
      agent: { id: 1, name: 'Agent', email: '', status: '', dateOfJoin: '' },
      customer: { id: 1, name: 'Customer', email: '', status: '', phone: '' },
      updatedAt: ""
    };
    component.setMessage(chat);
    expect(chatServiceMock.setActiveChat).toHaveBeenCalledWith(chat);
    expect(component.showChat.emit).toHaveBeenCalledWith(true);
  });

  it('should call connectToHub for each chat', () => {
    const chats: ChatModel[] = [
      {
        id: 10,
        issueName: 'Test',
        issueDescription: '',
        status: 'Active',
        AgentId: 1,
        CustomerId: 1,
        createdOn: '',
        agent: { id: 1, name: '', email: '', status: '', dateOfJoin: '' },
        customer: { id: 1, name: '', email: '', status: '', phone: '' }
      } as ChatModel
    ];
    component.connectToHub(chats);
    expect(chatHubServiceMock.joinChat).toHaveBeenCalledWith(10);
  });

  it('should not push notification if activeChat is the same chatId', () => {
    const message: Message = { id: 1, chatId: 99, };
    component.activeChat = { id: 99 } as ChatModel;
    component.pushNotification(message);
    expect(notificationServiceMock.addNewNotification).not.toHaveBeenCalled();
  });

  it('should push notification if activeChat is different', () => {
    const message: Message = { id: 1, chatId: 77, };
    component.activeChat = { id: 99 } as ChatModel;
    component.pushNotification(message);
    expect(notificationServiceMock.addNewNotification).toHaveBeenCalledWith(message);
  });
});
