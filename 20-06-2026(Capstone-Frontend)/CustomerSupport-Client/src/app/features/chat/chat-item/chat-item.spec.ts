import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChatItem } from './chat-item';
import { ChatModel } from '../../../core/models/chat';
import { AuthService } from '../../../core/services/auth-service';
import { ChatService } from '../../../core/services/chat-service';
import { NotificationService } from '../../../core/services/notification-service';
import { BehaviorSubject } from 'rxjs';
import { importProvidersFrom } from '@angular/core';
import { LucideAngularModule, TicketCheck, TicketPercent } from 'lucide-angular';

describe('ChatItem', () => {
  let component: ChatItem;
  let fixture: ComponentFixture<ChatItem>;

  let chatServiceMock: any;
  let authServiceMock: any;
  let notificationServiceMock: any;

  const testChat: ChatModel = {
    id: 1,
    issueName: 'Login issue',
    issueDescription: '',
    status: 'Active',
    AgentId: 1,
    CustomerId: 2,
    createdOn: '',
    agent: { id: 1, name: 'Agent A', email: '', status: '', dateOfJoin: '' },
    customer: { id: 2, name: 'Customer B', email: '', status: '', phone: '' },
    updatedAt: ""
  };

  beforeEach(async () => {
    chatServiceMock = {
      activeChat$: new BehaviorSubject<ChatModel | null>(null)
    };

    authServiceMock = {
      currentUser$: new BehaviorSubject({ role: 'Agent', username: 'agent1' })
    };

    notificationServiceMock = {
      notification$: new BehaviorSubject([]),
      removeNotifications: jasmine.createSpy('removeNotifications')
    };

    await TestBed.configureTestingModule({
      imports: [ChatItem],
      providers: [
        { provide: ChatService, useValue: chatServiceMock },
        { provide: AuthService, useValue: authServiceMock },
        { provide: NotificationService, useValue: notificationServiceMock },
        importProvidersFrom(LucideAngularModule.pick({TicketPercent, TicketCheck}))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ChatItem);
    component = fixture.componentInstance;
    component.chat = testChat;
    fixture.detectChanges();
  });

  it('should create ChatItem component', () => {
    expect(component).toBeTruthy();
  });

  it('should set newMessages to true if notification matches chat and not activeChat', () => {
    const message = { id: 10, chatId: 1, sender: '', content: '', timestamp: '' };
    notificationServiceMock.notification$.next([message]);
    expect(component.newMessages()).toBeTrue();
  });

  it('should set newMessages to false and call removeNotifications when chat becomes active', () => {
    chatServiceMock.activeChat$.next(testChat);
    expect(component.newMessages()).toBeFalse();
    expect(notificationServiceMock.removeNotifications).toHaveBeenCalledWith(1);
  });
});
