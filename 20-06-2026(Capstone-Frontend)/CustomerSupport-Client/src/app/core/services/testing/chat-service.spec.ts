import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ChatService } from '../chat-service';
import { ChatForm, ChatModel } from '../../models/chat';
import { Message } from '../../models/message';

describe('ChatService', () => {
    let service: ChatService;
    let httpMock: HttpTestingController;

    const mockChat: ChatModel = {
        id: 1,
        issueName: 'Login Issue',
        issueDescription: 'Cannot log in',
        AgentId: 1,
        CustomerId: 1,
        status: 'Active',
        createdOn: "",
        updatedAt: "",
        agent: { id: 1, name: 'Agent 1', email: '', dateOfJoin: '', status: 'Active' },
        customer: { id: 1, name: 'Customer 1', email: '', phone: "", status: 'Active' }
    };

    const mockMessage: Message = {
        id: 1,
        message: 'Test message',
        chatId: 1,
        createdAt: new Date().toISOString(),
        userId: 'user1'
    };

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule],
            providers: [ChatService]
        });

        service = TestBed.inject(ChatService);
        httpMock = TestBed.inject(HttpTestingController);

        // Handle constructor auto-triggered GET call
        const initReq = httpMock.expectOne('http://localhost:5124/api/v1/chat?pageSize=1000&pageNumber=1&status=&query=');
        initReq.flush([]);
    });

    afterEach(() => httpMock.verify());

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should fetch chats', () => {
        service.getChats().subscribe(data => {
            expect(data).toEqual([mockChat]);
        });

        const req = httpMock.expectOne('http://localhost:5124/api/v1/chat?pageSize=1000&pageNumber=1');
        expect(req.request.method).toBe('GET');
        req.flush([mockChat]);

        expect(service.chatSubject.value).toEqual([mockChat]);
    });

    it('should set active chat', () => {
        service.setActiveChat(mockChat);
        expect(service.activeChatSubject.value).toEqual(mockChat);
    });

    it('should fetch chat messages', () => {
        service.getChatMessages(1).subscribe(data => {
            expect(data).toEqual([mockMessage]);
        });

        const req = httpMock.expectOne('http://localhost:5124/api/v1/chat/1/message?pageSize=200');
        expect(req.request.method).toBe('GET');
        req.flush([mockMessage]);

        expect(service.messagesSubject.value).toEqual([mockMessage]);
    });

    it('should append message to messagesSubject', () => {
        service.appendMessages(mockMessage);
        expect(service.messagesSubject.value).toContain(mockMessage);
    });

    it('should send a text message', () => {
        service.sendTextMessage(1, 'Hello').subscribe();
        const req = httpMock.expectOne('http://localhost:5124/api/v1/chat/1/message');
        expect(req.request.method).toBe('POST');
        expect(req.request.body).toEqual({ message: 'Hello' });
        req.flush({});
    });

    it('should upload an image', () => {
        const mockFile = new File(['image'], 'test.jpg', { type: 'image/jpeg' });

        service.sendImage(1, mockFile).subscribe();

        const req = httpMock.expectOne('http://localhost:5124/api/v1/chat/1/image');
        expect(req.request.method).toBe('POST');
        expect(req.request.body.has('formFile')).toBeTrue();
        req.flush({});
    });

    it('should fetch chat image', () => {
        service.getChatImage(1, 'test.jpg').subscribe();
        const req = httpMock.expectOne('http://localhost:5124/api/v1/chat/1/image/test.jpg');
        expect(req.request.method).toBe('GET');
        req.flush({});
    });

    it('should create a new chat', () => {
        const form: ChatForm = {
            issueName: 'Test Issue',
            issueDescription: 'Detailed issue'
        };

        service.createChat(form).subscribe();

        const req = httpMock.expectOne('http://localhost:5124/api/v1/chat');
        expect(req.request.method).toBe('POST');
        expect(req.request.body).toEqual(form);
        req.flush({});
    });

    it('should close a chat', () => {
        service.closeChat(1).subscribe();

        const req = httpMock.expectOne('http://localhost:5124/api/v1/chat/1');
        expect(req.request.method).toBe('DELETE');
        req.flush({});
    });

    it('should filter chats using filterSubject', () => {
        service.filterSubject.next('Active');

        const req = httpMock.expectOne(req =>
            req.url === 'http://localhost:5124/api/v1/chat' &&
            req.params.get('status') === 'Active'
        );
        expect(req.request.method).toBe('GET');
        req.flush([mockChat]);

        expect(service.chatSubject.value).toEqual([mockChat]);
    });
});
