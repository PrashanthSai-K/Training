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

  onAddToCart(id:number|undefined){
    this.addToCart.emit(id);
  }
  // private productService = inject(ProductService);

  // constructor()
  // {
  //   this.productService.getProduct(1).subscribe({
  //     next :(data)=> {
  //       console.log(data);
  //       this.product = data as ProductModel;
  //     },
  //     error :(err)=> {
  //       console.log(err);
  //     },
  //     complete: ()=> {
  //       console.log("Api call completed");
  //     }
  //   })
  // }

}
