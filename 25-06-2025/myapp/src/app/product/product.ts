import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ProductService } from '../service/product-service';
import { ProductModel } from '../models/product';
import { CurrencyPipe, IMAGE_CONFIG, NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe, NgOptimizedImage],
  templateUrl: './product.html',
  styleUrl: './product.css',
  providers: [
  {
    provide: IMAGE_CONFIG,
    useValue: {
      placeholderResolution: 40
    }
  },
],
})
export class Product {
  @Input() product!: ProductModel;
  @Output() addToCart = new EventEmitter<any>();

  onAddToCart(event:Event, id:number|undefined){
    event.preventDefault();
    this.addToCart.emit(id);
  }
}
