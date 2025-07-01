import { ComponentFixture, TestBed } from '@angular/core/testing';
import { App } from './app';
import { Router, NavigationEnd } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, importProvidersFrom } from '@angular/core';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { AuthService } from './core/services/auth-service';
import { User } from './core/models/user';
import { NotificationService } from './core/services/notification-service';
import { BotMessageSquare, LayoutDashboard, LucideAngularModule, ShieldUser, UserCog } from 'lucide-angular';

describe('App', () => {
  let component: App;
  let fixture: ComponentFixture<App>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let routerEvents$: Subject<any>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    routerEvents$ = new Subject<NavigationEnd>();

    const mockUser: User = {
      username: 'Test User',
      role: 'Admin',
    };

    mockAuthService = jasmine.createSpyObj<AuthService>(
      'AuthService',
      ['getUser'],
      {
        currentUser$: of(mockUser),
        routeSubject: new BehaviorSubject<string>(''),
        route$: of('/dashboard')
      }
    );

    mockRouter = jasmine.createSpyObj<Router>('Router', [], {
      events: routerEvents$.asObservable()
    });

    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter },
        NotificationService,
        importProvidersFrom(LucideAngularModule.pick({ LayoutDashboard, ShieldUser, UserCog, BotMessageSquare }))
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(App);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the App component', () => {
    expect(component).toBeTruthy();
  });

  it('should toggle navigation', () => {
    const initial = component.isNavOpen();
    component.toggledNavbar(null);
    expect(component.isNavOpen()).toBe(!initial);
  });

  it('should toggle notification', () => {
    const initial = component.isNotificationOpen();
    component.toggleNotification(null);
    expect(component.isNotificationOpen()).toBe(!initial);
  });

  it('should hide nav for auth routes', () => {
    component.navChanged('/login');
    expect(component.isNavVisible()).toBeFalse();
  });

  it('should show nav for protected routes', () => {
    component.navChanged('/dashboard');
    expect(component.isNavVisible()).toBeTrue();
  });

  it('should set user from authService', () => {
    expect(component.user).toEqual(jasmine.objectContaining({ role: 'Admin' }));
  });
});
