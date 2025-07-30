import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Order {
  orderID: number;
  orderDate: string;
  paymentType: string;
  status: string;
  customerName: string;
  customerPhone: string;
  customerEmail: string;
  customerAddress: string;
  orderDetails: OrderDetail[];
}

export interface OrderDetail {
  quantity: number;
  price: number;
  productID: number
}

@Injectable({ providedIn: 'root' })
export class OrderService {
  private apiUrl = 'http://localhost:5038/api/order';

  constructor(private http: HttpClient) {}

  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl);
  }

  getOrderById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`);
  }

  downloadPdf(): Observable<Blob> {
  return this.http.get(`${this.apiUrl}/export`, {
    responseType: 'blob'
  });
}

}
