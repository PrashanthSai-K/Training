import { Component } from '@angular/core';
import { CartService } from '../Services/cart.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-checkout',
  imports: [CommonModule, FormsModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css'
})
export class Checkout {
  form = { name: '', phone: '', email: '', address: '' };

  constructor(private cart: CartService, private http: HttpClient, private router: Router) {}

  submit() {
    const order = {
      orderName: `Order_${new Date().getTime()}`,
      orderDate: new Date(),
      paymentType: 'Cash',
      customerName: this.form.name,
      customerPhone: this.form.phone,
      customerEmail: this.form.email,
      customerAddress: this.form.address,
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
