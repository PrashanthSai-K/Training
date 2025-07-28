import { Component } from '@angular/core';
import { CartItem, CartService } from '../Services/cart.service';
import { CommonModule, DecimalPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-shopping-cart',
  imports: [DecimalPipe, CommonModule, FormsModule, RouterLink],
  templateUrl: './shopping-cart.html',
  styleUrl: './shopping-cart.css'
})
export class ShoppingCart {
  cartItems: CartItem[] = [];
  total = 0;
  updateQuantity:number = 0;

  constructor(private cartService: CartService) {}

  ngOnInit() {
    this.cartService.cart$.subscribe(items => {
      this.cartItems = items;
      this.total = this.cartService.getTotal();
    });
  }

  updateQty(productId: number, event:any) {
    const qty = parseInt((event as HTMLInputElement).value , 10);
    if (qty > 0) {
      this.cartService.updateQuantity(productId, qty);
    }
  }

  remove(productId: number) {
    this.cartService.removeItem(productId);
  }

}
