import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AgentManagement } from './agent-management';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { of } from 'rxjs';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { AgentService } from '../../core/services/agent-service';
import { AuthService } from '../../core/services/auth-service';
import { Agent } from '../../core/models/chat';
import { importProvidersFrom } from '@angular/core';
import { LucideAngularModule, MoreVertical } from 'lucide-angular';

describe('AgentManagement', () => {
  let component: AgentManagement;
  let fixture: ComponentFixture<AgentManagement>;

  const mockAgent = {
    id: 1,
    name: 'Test Agent',
    email: 'agent@test.com',
    dateOfJoin: "2025-06-01",
    status: 'Active'
  };

  const agentServiceMock = {
    getAgents: jasmine.createSpy('getAgents').and.returnValue(of([mockAgent])),
    activateAgent: jasmine.createSpy('activateAgent').and.returnValue(of({})),
    deactivateAgent: jasmine.createSpy('deactivateAgent').and.returnValue(of({})),
    searchSubject: { next: jasmine.createSpy('searchSubject.next') },
    editingAgentSubject: { next: jasmine.createSpy('editingAgentSubject.next') },
    agents$: of([mockAgent])
  };

  const authServiceMock = {
    getUser: jasmine.createSpy('getUser').and.returnValue(of({ role: 'Admin' }))
  };

  const snackBarMock = {
    open: jasmine.createSpy('open')
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AgentManagement, MatSnackBarModule],
      providers: [
        { provide: AgentService, useValue: agentServiceMock },
        { provide: AuthService, useValue: authServiceMock },
        { provide: MatSnackBar, useValue: snackBarMock },
        provideNoopAnimations(),
        importProvidersFrom(LucideAngularModule.pick({MoreVertical}))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AgentManagement);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize and fetch agents', () => {
    expect(agentServiceMock.getAgents).toHaveBeenCalled();
    expect(component.agents.length).toBeGreaterThan(0);
  });

  it('should call searchSubject on search input', () => {
    component.searchQuery = 'John';
    component.onSearch();
    expect(agentServiceMock.searchSubject.next).toHaveBeenCalledWith('John');
  });

  it('should open and close create agent popup', () => {
    component.openCreateAgent();
    expect(component.isCreateAgentActive()).toBeTrue();

    component.closeAgentPopup();
    expect(component.isCreateAgentActive()).toBeFalse();
    expect(agentServiceMock.editingAgentSubject.next).toHaveBeenCalledWith(null);
  });

  it('should open and close edit agent popup', () => {
    component.openEditAgent(mockAgent);
    expect(component.isEditAgentActive()).toBeTrue();

    component.closeEditAgent();
    expect(component.isEditAgentActive()).toBeFalse();
    expect(agentServiceMock.editingAgentSubject.next).toHaveBeenCalledWith(null);
  });

  it('should activate agent with confirmation', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    component.activateAgent(mockAgent);
    expect(agentServiceMock.activateAgent).toHaveBeenCalledWith(mockAgent.id);
    expect(snackBarMock.open).toHaveBeenCalledWith('Agent account activated', '', { duration: 1000 });
  });

  it('should deactivate agent with confirmation', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    component.deactivateAgent(mockAgent);
    expect(agentServiceMock.deactivateAgent).toHaveBeenCalledWith(mockAgent.id);
    expect(snackBarMock.open).toHaveBeenCalledWith('Agent account deactivated', '', { duration: 1000 });
  });
});
