import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Product } from './product.Service';

export interface CartItem {
  productId: number;
  productName: string;
  price: number;
  quantity: number;
  image?: string;
}

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private cart = new BehaviorSubject<CartItem[]>([]);
  cart$ = this.cart.asObservable();

  addToCart(product: Product) {
    const items = this.cart.value;
    const existing = items.find(i => i.productId === product.productId);
    if (existing) {
      existing.quantity += 1;
    } else {
      var item = {
        productId: product.productId,
        productName: product.productName,
        price: product.price,
        quantity: 1,
        image: product.image
      }
      items.push(item);
    }
    this.cart.next([...items]);
  }

  updateQuantity(productId: number, quantity: number) {
    const items = this.cart.value.map(i =>
      i.productId === productId ? { ...i, quantity } : i
    );
    this.cart.next(items);
  }

  removeItem(productId: number) {
    this.cart.next(this.cart.value.filter(i => i.productId !== productId));
  }

  clearCart() {
    this.cart.next([]);
  }

  getItems() {
    return this.cart.value;
  }

  getTotal() {
    return this.cart.value.reduce((sum, i) => sum + i.price * i.quantity, 0);
  }
}
