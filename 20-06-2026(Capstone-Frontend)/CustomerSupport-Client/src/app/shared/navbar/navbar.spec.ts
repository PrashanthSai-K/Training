import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Navbar } from './navbar';
import { of, BehaviorSubject, Subject } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { Component, importProvidersFrom } from '@angular/core';
import { BotMessageSquare, LucideAngularModule } from 'lucide-angular';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth-service';
import { NotificationService } from '../../core/services/notification-service';
import { User } from '../../core/models/user';
import { Message } from '../../core/models/message';

describe('Navbar', () => {
  let component: Navbar;
  let fixture: ComponentFixture<Navbar>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockNotificationService: jasmine.SpyObj<NotificationService>;

  const mockUser: User = {
    username: 'Test User',
    role: 'Admin',
  };

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj<AuthService>('AuthService', ['logoutUser'], {
      currentUser$: of(mockUser),
      route$: new BehaviorSubject<string>('/dashboard')
    });

    mockNotificationService = jasmine.createSpyObj<NotificationService>('NotificationService', ['addNewNotification', 'removeNotifications'], {
      notification$: of([] as Message[])
    });

    await TestBed.configureTestingModule({
      imports: [Navbar, RouterTestingModule, LucideAngularModule, CommonModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: NotificationService, useValue: mockNotificationService },
        importProvidersFrom(LucideAngularModule.pick({ BotMessageSquare }))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Navbar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the Navbar component', () => {
    expect(component).toBeTruthy();
  });

  it('should emit toggledNavbar when toggleNavbar is called', () => {
    spyOn(component.toggledNavbar, 'emit');
    component.toggleNavbar();
    expect(component.toggledNavbar.emit).toHaveBeenCalled();
  });

  it('should emit toggledNotification when toggleNotification is called', () => {
    spyOn(component.toggledNotification, 'emit');
    component.toggleNotification();
    expect(component.toggledNotification.emit).toHaveBeenCalled();
  });

  it('should update isVisible based on route$', () => {
    const navSpy = spyOn(component.navDisabled, 'emit');
    (mockAuthService.route$ as BehaviorSubject<string>).next('/login');
    fixture.detectChanges();

    expect(component.isVisible).toBeFalse();
    expect(navSpy).toHaveBeenCalledWith('/login');
  });

  it('should call logoutUser on logout', () => {
    component.onLogout();
    expect(mockAuthService.logoutUser).toHaveBeenCalled();
  });

});
