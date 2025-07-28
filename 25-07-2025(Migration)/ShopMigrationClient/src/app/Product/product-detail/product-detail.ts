import { Component } from '@angular/core';
import { Product, ProductService } from '../../Services/product.Service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-product-detail',
  imports: [RouterLink, DecimalPipe, CommonModule],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.css'
})
export class ProductDetail {
  product!: Product;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.productService.getProduct(id).subscribe(p => (this.product = p));
  }

}
