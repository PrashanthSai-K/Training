import { Component } from '@angular/core';
import { Product, ProductService } from '../../Services/product.Service';
import { RouterLink } from '@angular/router';
import { CommonModule, DecimalPipe } from '@angular/common';
import { CartService } from '../../Services/cart.service';

@Component({
  selector: 'app-product-list',
  imports: [RouterLink, CommonModule, DecimalPipe],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css'
})
export class ProductList {
  products: Product[] = [];
  currentPage = 1;
  totalPages = 1;

  constructor(private productService: ProductService, private cartService: CartService) { 
    productService.productsSubject.subscribe({
      next:(data)=>{
        this.products = data;
      }
    })
  }

  ngOnInit(): void {
    this.loadPage(1);
  }

  loadPage(page: number) {
    this.productService.getProducts(page).subscribe(response => {
      this.products = response;
    });
  }

    addToCart(product: Product) {
    this.cartService.addToCart(product);
    alert(`${product.productName} added to cart`);
  }

}
