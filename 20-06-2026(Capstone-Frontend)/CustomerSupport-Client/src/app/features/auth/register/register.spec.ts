import { ComponentFixture, TestBed, tick } from '@angular/core/testing';
import { Register } from './register';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ReactiveFormsModule } from '@angular/forms';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { LucideAngularModule } from 'lucide-angular';
import { of, throwError } from 'rxjs';
import { CustomerService } from '../../../core/services/customer-service';
import { BotMessageSquare } from 'lucide-angular';

describe('Register', () => {
  let component: Register;
  let fixture: ComponentFixture<Register>;

  let routerMock: any;
  let snackBarMock: any;
  let customerServiceMock: any;

  beforeEach(async () => {
    routerMock = {
      navigateByUrl: jasmine.createSpy('navigateByUrl'),
    };

    snackBarMock = {
      open: jasmine.createSpy('open')
    };

    customerServiceMock = {
      registerCustomer: jasmine.createSpy('registerCustomer').and.returnValue(of({}))
    };

    await TestBed.configureTestingModule({
      imports: [Register, ReactiveFormsModule, MatSnackBarModule],
      providers: [
        { provide: Router, useValue: routerMock },
        { provide: MatSnackBar, useValue: snackBarMock },
        { provide: CustomerService, useValue: customerServiceMock },
        provideNoopAnimations(),
        importProvidersFrom(LucideAngularModule.pick({ BotMessageSquare }))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Register);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should mark the form invalid when empty', () => {
    component.registerForm.setValue({
      name: '',
      email: '',
      phone: '',
      password: ''
    });
    expect(component.registerForm.invalid).toBeTrue();
  });

  it('should show snackbar when submitting invalid form', () => {
    component.registerForm.setValue({
      name: '',
      email: '',
      phone: '',
      password: ''
    });
    component.onSubmit();
    expect(snackBarMock.open).toHaveBeenCalledWith('Enter valid data in the form');
    expect(customerServiceMock.registerCustomer).not.toHaveBeenCalled();
  });

  it('should call registerCustomer and redirect on successful register', () => {
    component.registerForm.setValue({
      name: 'Test User',
      email: 'test@example.com',
      phone: '9876543210',
      password: 'Password1'
    });

    component.onSubmit();

    expect(customerServiceMock.registerCustomer).toHaveBeenCalledWith({
      name: 'Test User',
      email: 'test@example.com',
      phone: '9876543210',
      password: 'Password1'
    });

    expect(snackBarMock.open).toHaveBeenCalledWith('Signed up successfully.', '', { duration: 2000 });
    expect(routerMock.navigateByUrl).toHaveBeenCalledWith('/login');
  });

  it('should redirect to login on redirectLogin()', () => {
    component.redirectLogin();
    expect(routerMock.navigateByUrl).toHaveBeenCalledWith('/login');
  });

});
