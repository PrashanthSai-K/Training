import { Component } from '@angular/core';
import { Order, OrderService } from '../../Services/order.service';
import { RouterLink } from '@angular/router';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-order-list',
  imports: [RouterLink, CommonModule, DatePipe],
  templateUrl: './order-list.html',
  styleUrl: './order-list.css'
})
export class OrderList {
  orders: Order[] = [];

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.orderService.getAllOrders().subscribe({
      next: (data) => this.orders = data,
      error: (err) => console.error(err),
    });
  }

}
