import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminSidebar } from './admin-sidebar';
import { of, BehaviorSubject } from 'rxjs';
import { CommonModule } from '@angular/common';
import { LucideAngularModule } from 'lucide-angular';
import { AuthService } from '../../core/services/auth-service';

describe('AdminSidebar', () => {
  let component: AdminSidebar;
  let fixture: ComponentFixture<AdminSidebar>;
  let mockAuthService: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj<AuthService>('AuthService', [], {
      route$: new BehaviorSubject<string>('/dashboard')
    });

    await TestBed.configureTestingModule({
      imports: [AdminSidebar, CommonModule, LucideAngularModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminSidebar);
    component = fixture.componentInstance;
    component.isNavOpen = true;
    fixture.detectChanges();
  });

  it('should create the AdminSidebar component', () => {
    expect(component).toBeTruthy();
  });

  it('should set initial url from route$', () => {
    expect(component.url()).toBe('/dashboard');
  });

  it('should update url when route$ emits new value', () => {
    const routeSubject = mockAuthService.route$ as BehaviorSubject<string>;
    routeSubject.next('/customer');
    fixture.detectChanges();

    expect(component.url()).toBe('/customer');
  });
});
