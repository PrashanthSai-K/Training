import { NotificationService } from '../notification-service';
import { Message } from '../../models/message';

describe('NotificationService', () => {
  let service: NotificationService;

  const mockMessage1: Message = {
    id: 1,
    message: 'Test message 1',
    chatId: 101,
    createdAt: "",
    userId: 'user1'
  };

  const mockMessage2: Message = {
    id: 2,
    message: 'Test message 2',
    chatId: 102,
    createdAt: "",
    userId: 'user2'
  };

  beforeEach(() => {
    service = new NotificationService();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initially have no notifications', (done) => {
    service.notification$.subscribe(notifications => {
      expect(notifications.length).toBe(0);
      done();
    });
  });

  it('should add a new notification', (done) => {
    service.addNewNotification(mockMessage1);

    service.notification$.subscribe(notifications => {
      expect(notifications.length).toBe(1);
      expect(notifications[0]).toEqual(mockMessage1);
      done();
    });
  });

  it('should add multiple notifications', (done) => {
    service.addNewNotification(mockMessage1);
    service.addNewNotification(mockMessage2);

    service.notification$.subscribe(notifications => {
      expect(notifications.length).toBe(2);
      expect(notifications).toContain(mockMessage1);
      expect(notifications).toContain(mockMessage2);
      done();
    });
  });

  it('should remove notifications by chatId', (done) => {
    service.addNewNotification(mockMessage1);
    service.addNewNotification(mockMessage2);

    // Remove notifications for chatId 101
    service.removeNotifications(101);

    service.notification$.subscribe(notifications => {
      expect(notifications.length).toBe(1);
      expect(notifications[0].chatId).toBe(102);
      done();
    });
  });
});
