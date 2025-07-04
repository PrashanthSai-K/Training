import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Payment } from './payment';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('Payment Component', () => {
  let component: Payment;
  let fixture: ComponentFixture<Payment>;
  let mockRzpInstance: any;


  beforeEach(async () => {
    mockRzpInstance = {
      on: jasmine.createSpy('on'),
      open: jasmine.createSpy('open'),
    };

    (window as any).Razorpay = function (options: any) {
      options.handler({ razorpay_payment_id: 'test_id' });
      return mockRzpInstance;
    };

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, CommonModule, Payment],
    }).compileComponents();


    fixture = TestBed.createComponent(Payment);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });


  it('should initialize form with empty values', () => {
    expect(component.form).toBeDefined();
    expect(component.form.valid).toBeFalse();
  });

  it('should invalidate form with missing fields', () => {
    component.form.patchValue({
      name: '',
      email: 'invalid-email',
      number: '',
      amount: null
    });
    expect(component.form.valid).toBeFalse();
  });


  it('should validate form with correct data', () => {
    component.form.patchValue({
      name: 'John',
      email: 'john@example.com',
      number: '9876543210',
      amount: 100
    });
    expect(component.form.valid).toBeTrue();
  });

  it('should reset the form on successful payment', () => {
    const alertSpy = spyOn(window, "alert")
    component.form.setValue({
      name: 'Test User',
      email: 'test@example.com',
      number: '1234567890',
      amount: 100
    });

    component.payWithRazorpay();
    expect(alertSpy).toHaveBeenCalledWith("Payment success test_id");
    expect(component.form.value).toEqual({
      name: null,
      email: null,
      number: null,
      amount: null
    });
  });

  it('should alert on payment failure', () => {
    const alertSpy = spyOn(window, 'alert');

    let failedCallback: any;
    (window as any).Razorpay = function (options: any) {
      return {
        on: (eventName: string, callback: any) => {
          if (eventName === 'payment.failed') {
            failedCallback = callback;
          }
        },
        open: () => { }
      };
    };

    component.form.setValue({
      name: 'Fail User',
      email: 'fail@example.com',
      number: '9876543210',
      amount: 300
    });

    component.payWithRazorpay();

    failedCallback({
      error: {
        code: 'BAD_REQUEST_ERROR',
        description: 'Payment was declined'
      }
    });

    expect(alertSpy).toHaveBeenCalledWith('❌ Payment Failed\nReason: Payment was declined');
  });

});
