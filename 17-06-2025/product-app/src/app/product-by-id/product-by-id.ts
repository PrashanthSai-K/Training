import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router, RouterLink } from '@angular/router';
import { ProductModel } from '../model/product';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ProductService } from '../service/product-service';

@Component({
  selector: 'app-product-by-id',
  imports: [RouterLink, CurrencyPipe, CommonModule],
  templateUrl: './product-by-id.html',
  styleUrl: './product-by-id.css'
})
export class ProductById implements OnInit {
  pId: number = 0;
  product!: ProductModel;

  private route: ActivatedRoute = inject(ActivatedRoute);
  private productService: ProductService = inject(ProductService);
  private router: Router = inject(Router);

  constructor() {
  }

  ngOnInit(): void {
    this.pId = this.route.snapshot.params["id"];
    if (!this.pId || isNaN(this.pId)) {
      alert("Not a valid Id");
      this.router.navigateByUrl("products");
    }
    this.productService.getProductById(this.pId).subscribe({
      next: (data) => {
        this.product = data as ProductModel;
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
}
