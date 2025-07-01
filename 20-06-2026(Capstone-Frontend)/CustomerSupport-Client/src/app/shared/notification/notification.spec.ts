import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Notification } from './notification';
import { of, BehaviorSubject } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Message } from '../../core/models/message';
import { NotificationService } from '../../core/services/notification-service';

describe('Notification', () => {
  let component: Notification;
  let fixture: ComponentFixture<Notification>;
  let mockNotificationService: jasmine.SpyObj<NotificationService>;

  const mockMessages: Message[] = [
    { id: 1, chatId: 1, userId: "test1", message: 'Test 1', },
    { id: 2, chatId: 2, userId: "test2", message: 'Test 2', }
  ];

  beforeEach(async () => {
    mockNotificationService = jasmine.createSpyObj<NotificationService>('NotificationService', [], {
      notification$: new BehaviorSubject<Message[]>(mockMessages)
    });

    await TestBed.configureTestingModule({
      imports: [Notification, CommonModule],
      providers: [
        { provide: NotificationService, useValue: mockNotificationService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Notification);
    component = fixture.componentInstance;
    component.isVisible = true;
    fixture.detectChanges();
  });

  it('should create the Notification component', () => {
    expect(component).toBeTruthy();
  });

  it('should receive notifications from the service', () => {
    expect(component.notifications.length).toBe(2);
    expect(component.notifications[0].userId).toBe("test1");
  });

  it('should show notification div if isVisible is true', () => {
    const notificationDiv = fixture.nativeElement.querySelector('div');
    expect(notificationDiv).toBeTruthy();
  });
});
