import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerManagement } from './customer-management';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { of } from 'rxjs';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { CustomerService } from '../../core/services/customer-service';
import { Customer } from '../../core/models/chat';
import { CustomerModel } from '../../core/models/register';
import { importProvidersFrom } from '@angular/core';
import { LucideAngularModule, MoreVertical } from 'lucide-angular';

const mockCustomers = [
  { id: 1, name: 'John Doe', email: 'john@example.com', phone: '1234567890', status: 'Active' },
  { id: 2, name: 'Jane Smith', email: 'jane@example.com', phone: '9876543210', status: 'Inactive' }
];

const mockCustomerService = {
  customer$: of(mockCustomers),
  searchSubject: { next: jasmine.createSpy('next') },
  editingCustomerSubject: { next: jasmine.createSpy('next') },
  getCustomers: () => of(mockCustomers),
  activateCustomer: () => of({}),
  deactivateCustomer: () => of({})
};

describe('CustomerManagement', () => {
  let component: CustomerManagement;
  let fixture: ComponentFixture<CustomerManagement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerManagement, MatSnackBarModule],
      providers: [
        { provide: CustomerService, useValue: mockCustomerService },
        { provide: MatSnackBar, useValue: { open: jasmine.createSpy('open') } },
        provideNoopAnimations(),
        importProvidersFrom(LucideAngularModule.pick({MoreVertical}))
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerManagement);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load customer data on init', () => {
    expect(component.customers.length).toBe(2);
    expect(component.customers[0].name).toBe('John Doe');
  });

  it('should call searchSubject on input', () => {
    component.searchQuery = 'test';
    component.onSearch();
    expect(mockCustomerService.searchSubject.next).toHaveBeenCalledWith('test');
  });

  it('should call activateCustomer and refresh list', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    const customer = mockCustomers[1];
    const refreshSpy = spyOn(mockCustomerService, 'getCustomers').and.callThrough();

    component.activateCustomer(customer as CustomerModel);

    expect(refreshSpy).toHaveBeenCalled();
  });

  it('should call deactivateCustomer and refresh list', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    const customer = mockCustomers[0];
    const refreshSpy = spyOn(mockCustomerService, 'getCustomers').and.callThrough();

    component.deactivateCustomer(customer as CustomerModel);

    expect(refreshSpy).toHaveBeenCalled();
  });

  it('should set create/edit popup states correctly', () => {
    component.openCreateCustomer();
    expect(component.isCreateCustomerActive()).toBeTrue();

    component.openEditCustomer(mockCustomers[0] as CustomerModel);
    expect(component.isEditCustomerActive()).toBeTrue();

    component.closeEditPopup();
    expect(component.isEditCustomerActive()).toBeFalse();
  });
});
