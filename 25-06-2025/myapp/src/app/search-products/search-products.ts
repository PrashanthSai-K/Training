import { Component } from '@angular/core';
import { debounceTime, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { ProductModel } from '../models/product';
import { ProductService } from '../service/product-service';
import { CartItem } from '../models/cart-item';
import { FormsModule } from '@angular/forms';
import { Product } from "../product/product";

@Component({
  selector: 'app-search-products',
  imports: [FormsModule, Product],
  templateUrl: './search-products.html',
  styleUrl: './search-products.css'
})
export class SearchProducts {
  products: ProductModel[] | undefined = undefined;
  cartItems: any[] = [];
  cartCount: number = 0;
  searchString: string = "";
  searchSubject = new Subject<string>();
  loading: boolean = false;
  constructor(private productService: ProductService) {

  }
  handleSearchProducts() {
    console.log(this.searchString)
    this.searchSubject.next(this.searchString);
  }

  handleAddToCart(event: number) {
    console.log("Handling add to cart - " + event)
    let flag = false;
    for (let i = 0; i < this.cartItems.length; i++) {
      if (this.cartItems[i].Id == event) {
        this.cartItems[i].Count++;
        flag = true;
      }
    }
    if (!flag)
      this.cartItems.push(new CartItem(event, 1));
    this.cartCount++;
  }
  ngOnInit(): void {
    this.productService.getAllProducts().subscribe(
      {
        next: (data: any) => {
          this.products = data.products as ProductModel[];
        },
        error: (err) => { },
        complete: () => { }
      }
    )
    this.searchSubject.pipe(
      debounceTime(2000),
      distinctUntilChanged(),
      tap(() => this.loading = true),
      switchMap(query => this.productService.getProductSerchResult(query)),
      tap(() => this.loading = false)
    ).subscribe({
      next: (data: any) => { this.products = data.products as ProductModel[]; },
      complete: () => console.log("Search completed")
    });
  }
}
