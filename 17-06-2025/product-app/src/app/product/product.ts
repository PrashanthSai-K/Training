import { Component, inject, Input } from '@angular/core';
import { ProductModel } from '../model/product';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { HighlightPipe } from '../pipes/highlight-pipe';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe, CommonModule, HighlightPipe],
  templateUrl: './product.html',
  styleUrl: './product.css'
})

export class Product {
  @Input() product!: ProductModel;
  @Input() searchQuery:string|null = null;
  router:Router = inject(Router);

  navigateProduct(id:number):void{
    this.router.navigateByUrl("products/"+id);
  }
}
