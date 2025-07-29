import { Component } from '@angular/core';
import { Product, ProductService } from '../../Services/product.Service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule, DecimalPipe } from '@angular/common';
import { CartService } from '../../Services/cart.service';

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
    private productService: ProductService,
    private cartService: CartService
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.productService.getProduct(id).subscribe(p => (this.product = p));
  }
  
  addToCart(product: Product) {
    this.cartService.addToCart(product);
    alert(`${product.productName} added to cart`);
  }

}
