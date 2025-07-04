import { CommonModule } from '@angular/common';
import { Component, NgZone } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {MatSnackBar} from '@angular/material/snack-bar';

declare var Razorpay: any;

@Component({
  selector: 'app-payment',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './payment.html',
  styleUrl: './payment.css'
})

export class Payment {

  form = new FormGroup({
    name: new FormControl("", [Validators.required]),
    email: new FormControl("", [Validators.required, Validators.email]),
    number: new FormControl("", [Validators.required, Validators.minLength(10)]),
    amount: new FormControl(0, [Validators.required, Validators.min(1)])
  });

  public get name(): any {
    return this.form.get('name')
  }
  public get email(): any {
    return this.form.get('email')
  }
  public get number(): any {
    return this.form.get('number')
  }
  public get amount(): any {
    return this.form.get('amount')
  }

  payWithRazorpay() {
    if (this.form.invalid) {
      return alert("Enter valid details.");
    }

    const options = {
      key: 'rzp_test_Fnr8qymkfdrY4q',
      amount: Number(this.form.get('amount')?.value) * 100,
      currency: 'INR',
      name: 'UPI Pay - Simulation',
      description: 'Sample UPI Transactions',
      orserId: "1234567890",
      handler: (response: any) => {
        alert(`Payment success ${response.razorpay_payment_id}`);
        this.form.reset();
        this.form.markAsPristine();
        this.form.markAsUntouched();
      },
      modal: {
        ondismiss: () => {
          alert(`payment Cancelled`)
        },
      },

      prefill: {
        method: 'upi',
        upi: {
          vpa: "success@razorpay",
        },
      },
      theme: {
        color: '#1c1c1c',
      },
    };

    const rzp = new (window as any).Razorpay(options);
    rzp.on('payment.failed', function (response: any) {
      alert('❌ Payment Failed\nReason: ' + response.error.description);
      console.error(response.error);
    });

    rzp.open();
  }

}
