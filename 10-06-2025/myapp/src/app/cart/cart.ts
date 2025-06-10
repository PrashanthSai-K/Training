import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-cart',
  imports: [CommonModule],
  templateUrl: './cart.html',
  styleUrl: './cart.css'
})
export class Cart {
  cartCount:number = 0;
  products: Product[] = [
    {name : "Nike Air Jordan 1", price: 10700, image:"assets/2.png", quantity : 5},
    {name : "Nike Pegasus 41", price: 7650, image: "assets/1.png", quantity: 4},
    {name: "Nike Zoom Fly 6", price: 15990, image: "assets/3.png", quantity: 5}
  ];
  onAddToCart(product:Product){
    if(product.quantity > 0)
    {
      this.cartCount++;
      product.quantity--;
    }else
      alert(`${product.name} is out of stock`)
  }
}
interface Product {
  name : string;
  price: number;
  image: string;
  quantity: number;
}