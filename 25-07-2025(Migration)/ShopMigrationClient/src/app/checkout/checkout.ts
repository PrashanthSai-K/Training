import { AfterViewInit, Component } from '@angular/core';
import { CartService } from '../Services/cart.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css'
})
export class Checkout implements AfterViewInit {
  checkoutForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private cart: CartService,
    private http: HttpClient,
    private router: Router
  ) {
    this.checkoutForm = this.fb.group({
      name: ['', Validators.required],
      phone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
    });
  }

  ngAfterViewInit(): void {
    this.loadPaypalScript().then(() => {
      this.renderPaypalButtons();
    });
  }

  private loadPaypalScript(): Promise<void> {
    return new Promise((resolve, reject) => {
      if ((window as any).paypal) {
        resolve();
        return;
      }

      const script = document.createElement('script');
      script.src = 'https://www.paypal.com/sdk/js?client-id=AZ0-ImIw25FPuXHmXzlZCUxvvWkLCDg9z3gFvFwgVmEpZhkV-IwW7VBh6e_diWHuN4Q6xG17KTbssL4c&currency=USD';
      script.onload = () => resolve();
      script.onerror = () => reject('PayPal SDK could not be loaded.');
      document.body.appendChild(script);
    });
  }

  private renderPaypalButtons(): void {
    const cartItems = this.cart.getItems();

    (window as any).paypal.Buttons({
      createOrder: (data: any, actions: any) => {
        if (this.checkoutForm.invalid) {
          this.checkoutForm.markAllAsTouched();
          alert('Please fill in all required fields before proceeding.');
          throw new Error('Form invalid');
        }

        const total = cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);
        return actions.order.create({
          purchase_units: [{
            amount: {
              value: total.toFixed(2),
            }
          }]
        });
      },

      onApprove: (data: any, actions: any) => {
        return actions.order.capture().then(() => {
          this.submit();
        });
      },

      onError: (err: any) => {
        console.error('PayPal Error:', err);
      }

    }).render('#paypal-button-container');
  }

  submit() {
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      return;
    }

    const order = {
      orderName: `Order_${new Date().getTime()}`,
      orderDate: new Date(),
      paymentType: 'PayPal',
      customerName: this.checkoutForm.value.name,
      customerPhone: this.checkoutForm.value.phone,
      customerEmail: this.checkoutForm.value.email,
      customerAddress: this.checkoutForm.value.address,
      orderDetailsDto: this.cart.getItems().map(i => ({
        productID: i.productId,
        price: i.price,
        quantity: i.quantity,
      })),
    };

    this.http.post('http://localhost:5038/api/order', order).subscribe({
      next: () => {
        this.cart.clearCart();
        this.router.navigate(['/order-success']);
      },
      error: () => {
        this.router.navigate(['/order-failure']);
      },
    });
  }
}
