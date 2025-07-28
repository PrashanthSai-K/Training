import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Order, OrderService } from '../../Services/order.service';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-order-detail',
  imports: [CommonModule, DatePipe, DecimalPipe, RouterLink],
  templateUrl: './order-detail.html',
  styleUrl: './order-detail.css'
})
export class OrderDetail {
  order!: Order;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadOrder(+id);
    }
  }

  loadOrder(id: number): void {
    this.orderService.getOrderById(id).subscribe({
      next: (data) => this.order = data,
      error: (err) => console.error(err),
    });
  }

}
