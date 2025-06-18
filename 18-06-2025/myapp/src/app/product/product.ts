import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ProductService } from '../service/product-service';
import { ProductModel } from '../models/product';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product {
  @Input() product: ProductModel | null = new ProductModel()
  @Output() addToCart = new EventEmitter<any>();

  onAddToCart(event:Event, id:number|undefined){
    event.preventDefault();
    this.addToCart.emit(id);
  }
}
