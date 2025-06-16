import { Component, Input } from '@angular/core';
import { ProductModel } from '../model/product';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { HighlightPipe } from '../pipes/highlight-pipe';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe, CommonModule, HighlightPipe],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product {
  @Input() product!: ProductModel;
  @Input() searchQuery:string|null = null;
}
