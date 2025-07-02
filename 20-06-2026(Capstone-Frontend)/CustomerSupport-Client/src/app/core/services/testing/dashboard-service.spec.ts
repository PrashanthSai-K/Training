import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DashboardService } from '../dashboard-service';

describe('DashboardService', () => {
  let service: DashboardService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [DashboardService]
    });

    service = TestBed.inject(DashboardService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch admin summary', () => {
    const mockResponse = { totalAgents: 10, totalChats: 50 };

    service.getAdminSummary().subscribe(data => {
      expect(data).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('http://localhost:5124/api/v1/dashboard/adminSummary');
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should fetch admin chat trend', () => {
    const mockResponse = [{ date: '2023-01-01', chats: 5 }];

    service.getAdminChatTrend().subscribe(data => {
      expect(data).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('http://localhost:5124/api/v1/dashboard/chatTrend');
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should fetch user summary', () => {
    const mockResponse = { userChats: 20, userActivity: 85 };

    service.getUserSummary().subscribe(data => {
      expect(data).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('http://localhost:5124/api/v1/dashboard/userSummary');
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should fetch user chat trend', () => {
    const mockResponse = [{ date: '2023-01-01', chats: 3 }];

    service.getUserChatTrend().subscribe(data => {
      expect(data).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('http://localhost:5124/api/v1/dashboard/userChatTrend');
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });
});
