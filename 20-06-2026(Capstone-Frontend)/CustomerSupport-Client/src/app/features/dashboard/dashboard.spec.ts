import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Dashboard } from './dashboard';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AsyncPipe } from '@angular/common';
import { AuthService } from '../../core/services/auth-service';
import { DashboardService } from '../../core/services/dashboard-service';
import { provideNoopAnimations } from '@angular/platform-browser/animations';


describe('Dashboard', () => {
  let component: Dashboard;
  let fixture: ComponentFixture<Dashboard>;

  const mockAdminUser = { role: 'Admin', username: 'Admin User' };

  const mockSummary = {
    agent: 10,
    customer: 20,
    chatCount: 30,
    activeChat: 15,
    closedChat: 15
  };

  const mockTrend = [
    { status: 'Open', date: '2024-01-01' },
    { status: 'Closed', date: '2024-01-01' },
    { status: 'Closed', date: '2024-01-02' }
  ];

  const mockAuthService = {
    getUser: jasmine.createSpy('getUser').and.returnValue(of(mockAdminUser)),
    currentUser$: of(mockAdminUser)
  };

  const mockDashboardService = {
    getAdminSummary: jasmine.createSpy('getAdminSummary').and.returnValue(of(mockSummary)),
    getAdminChatTrend: jasmine.createSpy('getAdminChatTrend').and.returnValue(of(mockTrend)),
    getUserSummary: jasmine.createSpy('getUserSummary'),
    getUserChatTrend: jasmine.createSpy('getUserChatTrend')
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Dashboard, CommonModule, NgxChartsModule, AsyncPipe],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: DashboardService, useValue: mockDashboardService },
        provideNoopAnimations()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Dashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch admin summary and trend data on init', fakeAsync(() => {
    tick(); 
    expect(mockAuthService.getUser).toHaveBeenCalled();
    expect(mockDashboardService.getAdminSummary).toHaveBeenCalled();
    expect(mockDashboardService.getAdminChatTrend).toHaveBeenCalled();
    expect(component.summary).toEqual(mockSummary);
    expect(component.trends.length).toBeGreaterThan(0);
    expect(component.isLoading()).toBeFalse();
  }));

  it('should transform raw trend data to ngx-chart format', () => {
    const result = component.transformToNgxChartFormat(mockTrend);
    expect(result.length).toBe(2); 
    expect(result[0].name).toBe('Open');
    expect(result[1].name).toBe('Closed');
    expect(result[1].series.length).toBe(2);
  });
});
