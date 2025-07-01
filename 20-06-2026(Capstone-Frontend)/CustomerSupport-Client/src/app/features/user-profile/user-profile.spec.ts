import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserProfile } from './user-profile';
import { of } from 'rxjs';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CircleUser, LucideAngularModule, SquarePen } from 'lucide-angular';
import { AuthService } from '../../core/services/auth-service';
import { AgentService } from '../../core/services/agent-service';
import { CustomerService } from '../../core/services/customer-service';
import { importProvidersFrom } from '@angular/core';

describe('UserProfile', () => {
  let component: UserProfile;
  let fixture: ComponentFixture<UserProfile>;

  const mockAuthService = {
    currentUser$: of({ role: 'Agent', username: 'Test Agent' }),
    getUser: jasmine.createSpy('getUser'),
  };

  const mockAgentService = {
    getAgent: jasmine.createSpy('getAgent').and.returnValue(of({
      id: 1,
      name: 'Test Agent',
      dateOfJoin: '2024-01-01T00:00:00.000Z'
    })),
    updateAgent: jasmine.createSpy('updateAgent').and.returnValue(of({}))
  };

  const mockCustomerService = {
    getCustomer: jasmine.createSpy('getCustomer').and.returnValue(of({
      id: 2,
      name: 'Customer User',
      phone: '1234567890'
    })),
    updateCustomer: jasmine.createSpy('updateCustomer').and.returnValue(of({}))
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        UserProfile,
        MatSnackBarModule,
        ReactiveFormsModule,
        CommonModule,
        LucideAngularModule
      ],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: AgentService, useValue: mockAgentService },
        { provide: CustomerService, useValue: mockCustomerService },
        importProvidersFrom(LucideAngularModule.pick({CircleUser, SquarePen}))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(UserProfile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with agent data', () => {
    expect(component.editingId).toBe(1);
    expect(component.userform.value.name).toBe('Test Agent');
    expect(component.userform.value.dateOfJoin).toBe('2024-01-01');
  });

  it('should enable form controls on edit', () => {
    component.enableEditing();
    expect(component.userform.get('name')?.enabled).toBeTrue();
    expect(component.userform.get('phone')?.enabled).toBeTrue();
    expect(component.userform.get('dateOfJoin')?.enabled).toBeTrue();
  });

  it('should disable form controls', () => {
    component.enableEditing();
    component.disableEditing();
    expect(component.userform.get('name')?.disabled).toBeTrue();
    expect(component.userform.get('phone')?.disabled).toBeTrue();
    expect(component.userform.get('dateOfJoin')?.disabled).toBeTrue();
  });

  it('should submit agent update', () => {
    component.enableEditing();
    component.onSubmit();
    expect(mockAgentService.updateAgent).toHaveBeenCalled();
    expect(mockAuthService.getUser).toHaveBeenCalled();
  });
});
