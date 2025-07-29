import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';

export interface Product {
  productId: number;
  productName: string;
  price: number;
  image: string;
}

@Injectable({ providedIn: 'root' })
export class ProductService {
  private baseUrl = 'http://localhost:5038/api/products';
  category?: number= 5;
  productsSubject = new BehaviorSubject<Product[]>([]);

  constructor(private http: HttpClient) { }

  getProducts(page: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/by-category/${this.category}?page=${page}`).pipe(
      tap(data => {
        this.productsSubject.next(data);
      })
    );
  }

  getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/${id}`);
  }
}
