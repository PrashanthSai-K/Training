import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateAgent } from './create-agent';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ReactiveFormsModule } from '@angular/forms';
import { of, Subject } from 'rxjs';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { AgentService } from '../../../core/services/agent-service';
import { importProvidersFrom } from '@angular/core';
import { LucideAngularModule, UserPlus } from 'lucide-angular';

describe('CreateAgent', () => {
  let component: CreateAgent;
  let fixture: ComponentFixture<CreateAgent>;
  let mockAgentService: any;
  let snackBarMock: any;

  const mockAgent = {
    id: 1,
    name: 'Agent Test',
    email: 'agent@test.com',
    dateOfJoin: new Date().toISOString(),
    status: 'Active'
  };

  beforeEach(async () => {
    const editingAgentSubject = new Subject<any>();

    mockAgentService = {
      editingAgentSubject: editingAgentSubject,
      editingAgent$: editingAgentSubject.asObservable(),
      registerCustomer: jasmine.createSpy('registerCustomer').and.returnValue(of({})),
      updateAgent: jasmine.createSpy('updateAgent').and.returnValue(of({})),
      getAgents: jasmine.createSpy('getAgents').and.returnValue(of([mockAgent])),
    };

    snackBarMock = {
      open: jasmine.createSpy('open')
    };

    await TestBed.configureTestingModule({
      imports: [CreateAgent, MatSnackBarModule, ReactiveFormsModule],
      providers: [
        { provide: AgentService, useValue: mockAgentService },
        { provide: MatSnackBar, useValue: snackBarMock },
        provideNoopAnimations(),
        importProvidersFrom(LucideAngularModule.pick({ UserPlus, }))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateAgent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should mark form as invalid if required fields are missing', () => {
    component.agentForm.setValue({ name: '', email: '', dateOfJoin: '' });
    expect(component.agentForm.invalid).toBeTrue();
  });

  it('should generate a valid random password', () => {
    const password = component.generateRandomPassword(8);
    expect(password.length).toBe(8);
    expect(/[A-Z]/.test(password)).toBeTrue();
    expect(/[a-z]/.test(password)).toBeTrue();
    expect(/[0-9]/.test(password)).toBeTrue();
    expect(/[!@#$%^&]/.test(password)).toBeTrue();
  });

  it('should submit create form successfully and show password', () => {
    spyOn(component, 'generateRandomPassword').and.returnValue('Abc123!@');
    component.agentForm.setValue({
      name: 'Test Agent',
      email: 'agent@example.com',
      dateOfJoin: '2023-01-01'
    });

    component.submitForm();
    expect(mockAgentService.registerCustomer).toHaveBeenCalled();
    expect(snackBarMock.open).toHaveBeenCalledWith('Agent has been created', '', { duration: 1000 });
    expect(component.showPassword()).toBeTrue();
  });

  it('should not submit form if invalid', () => {
    component.agentForm.setValue({
      name: '',
      email: 'invalid',
      dateOfJoin: ''
    });
    component.submitForm();
    expect(mockAgentService.registerCustomer).not.toHaveBeenCalled();
    expect(snackBarMock.open).toHaveBeenCalledWith('Enter valid details', '', { duration: 1000 });
  });

  it('should submit edit form successfully', () => {
    component.editingAgent = { ...mockAgent };
    component.agentForm.setValue({
      name: 'Edited Agent',
      email: 'edited@example.com',
      dateOfJoin: '2023-01-01'
    });
    component.submitForm();
    expect(mockAgentService.updateAgent).toHaveBeenCalledWith(jasmine.any(Object), mockAgent.id);
    expect(snackBarMock.open).toHaveBeenCalledWith('Agent has been updated', '', { duration: 1000 });
  });

  it('should patch agent form if editingAgent is provided', () => {
    const newAgent = {
      id: 2,
      name: 'New Edit',
      email: 'new@edit.com',
      dateOfJoin: '2023-06-01T00:00:00Z'
    };
    (mockAgentService.editingAgentSubject as Subject<any>).next(newAgent);
    fixture.detectChanges();

    expect(component.editingAgent?.name).toBe('New Edit');
    expect(component.agentForm.value.name).toBe('New Edit');
  });
});
