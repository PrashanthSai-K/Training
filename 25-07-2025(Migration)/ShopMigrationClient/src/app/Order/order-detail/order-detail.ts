import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Order, OrderService } from '../../Services/order.service';
import { AsyncPipe, CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { Product, ProductService } from '../../Services/product.Service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-order-detail',
  imports: [CommonModule, DatePipe, DecimalPipe, RouterLink, AsyncPipe],
  templateUrl: './order-detail.html',
  styleUrl: './order-detail.css'
})
export class OrderDetail {
  order!: Order;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    public productService: ProductService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadOrder(+id);
    }
  }
  productMap = new Map<number, Observable<Product>>();

  getProductById(id: number): Observable<Product> {
    if (!this.productMap.has(id)) {
      this.productMap.set(id, this.productService.getProduct(id));
    }
    return this.productMap.get(id)!;
  }

  loadOrder(id: number): void {
    this.orderService.getOrderById(id).subscribe({
      next: (data) => this.order = data,
      error: (err) => console.error(err),
    });
  }

}
