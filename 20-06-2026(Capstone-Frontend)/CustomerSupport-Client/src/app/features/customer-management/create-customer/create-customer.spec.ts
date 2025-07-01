import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateCustomer } from './create-customer';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { of } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { EventEmitter, importProvidersFrom } from '@angular/core';
import { CustomerService } from '../../../core/services/customer-service';
import { CustomerModel } from '../../../core/models/register';
import { LucideAngularModule, UserPlus } from 'lucide-angular';

describe('CreateCustomer', () => {
  let component: CreateCustomer;
  let fixture: ComponentFixture<CreateCustomer>;

  const mockCustomer = {
    id: 1,
    name: 'John Doe',
    email: 'john@example.com',
    phone: '9876543210',
    status: 'Active'
  };

  const customerServiceMock = {
    editingCustomer$: of(mockCustomer),
    registerCustomer: jasmine.createSpy('registerCustomer').and.returnValue(of({})),
    updateCustomer: jasmine.createSpy('updateCustomer').and.returnValue(of({})),
    getCustomers: jasmine.createSpy('getCustomers').and.returnValue(of([]))
  };

  const snackBarMock = {
    open: jasmine.createSpy('open')
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateCustomer, MatSnackBarModule, ReactiveFormsModule],
      providers: [
        { provide: CustomerService, useValue: customerServiceMock },
        { provide: MatSnackBar, useValue: snackBarMock },
        provideNoopAnimations(),
        importProvidersFrom(LucideAngularModule.pick({UserPlus}))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateCustomer);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with customer data if editing', () => {
    expect(component.editingCustomer).toEqual(mockCustomer as CustomerModel);
    expect(component.customerForm.value.name).toBe('John Doe');
    expect(component.customerForm.value.email).toBe('john@example.com');
    expect(component.customerForm.value.phone).toBe('9876543210');
  });

  it('should generate a password of expected length and structure', () => {
    const pwd = component.generateRandomPassword(10);
    expect(pwd.length).toBe(10);
    expect(/[A-Z]/.test(pwd)).toBeTrue();
    expect(/[a-z]/.test(pwd)).toBeTrue();
    expect(/[0-9]/.test(pwd)).toBeTrue();
    expect(/[!@#$%^&]/.test(pwd)).toBeTrue();
  });

  it('should call updateCustomer on edit submit', () => {
    component.customerForm.setValue({ name: 'Jane', email: 'jane@example.com', phone: '9876543210' });
    component.editingCustomer = mockCustomer as CustomerModel;
    component.submitForm();

    expect(customerServiceMock.updateCustomer).toHaveBeenCalledWith(jasmine.objectContaining({
      id: 1,
      name: 'Jane',
      phone: '9876543210'
    }));
    expect(snackBarMock.open).toHaveBeenCalledWith('Customer has been updated', '', { duration: 1000 });
  });

  it('should call registerCustomer on create submit', () => {
    component.editingCustomer = null;
    component.customerForm.setValue({ name: 'Alice', email: 'alice@example.com', phone: '9876543210' });
    component.submitForm();

    expect(customerServiceMock.registerCustomer).toHaveBeenCalledWith(
      jasmine.objectContaining({ name: 'Alice' }),
      jasmine.any(String)
    );
    expect(snackBarMock.open).toHaveBeenCalledWith('Customer has been created', '', { duration: 1000 });
  });

  it('should show snackbar if form is invalid on submit', () => {
    component.customerForm.setValue({ name: '', email: 'invalid', phone: '' });
    component.editingCustomer = null;
    component.submitForm();
    expect(snackBarMock.open).toHaveBeenCalledWith('Enter valid details', '', { duration: 1000 });
  });

  it('should copy password to clipboard', async () => {
    const mockPassword = 'Abc@1234';
    component.password.set(mockPassword);
    spyOn(navigator.clipboard, 'writeText').and.callFake(() => Promise.resolve());

    await component.copyPasswordToClipboard();

    expect(navigator.clipboard.writeText).toHaveBeenCalledWith(mockPassword);
    expect(snackBarMock.open).toHaveBeenCalledWith('Copied to clipboard', '', { duration: 1000 });
  });
});
