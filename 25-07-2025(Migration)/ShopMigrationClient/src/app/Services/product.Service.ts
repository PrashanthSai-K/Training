import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Product {
  productId: number;
  productName: string;
  price: number;
  image: string;
}

@Injectable({ providedIn: 'root' })
export class ProductService {
  private baseUrl = 'http://localhost:5038/api/products';

  constructor(private http: HttpClient) {}

  getProducts(page: number, category?: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}?page=${page}`);
  }

  getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/${id}`);
  }
}
