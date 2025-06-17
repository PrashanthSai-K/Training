import { Component, HostListener, OnInit } from '@angular/core';
import { debounceTime, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { ProductModel } from '../model/product';
import { ProductService } from '../service/product-service';
import { FormsModule } from '@angular/forms';
import { Product } from '../product/product';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [FormsModule, Product, RouterOutlet],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {

  products: ProductModel[] = [];
  searchQuery = "";
  searchQuerySub = new Subject<string>();
  loading: boolean = false;
  total = 194;
  limit = 16;
  skip = 0;

  constructor(private productService: ProductService) {
  }

  handleSearch() {
    console.log(this.searchQuery);
    this.searchQuerySub.next(this.searchQuery);
  }

  ngOnInit(): void {
    this.productService.getProducts("").subscribe({
      next: (data: any) => this.products = data.products as ProductModel[],
      error: (err) => console.log(err)
    })
    this.searchQuerySub.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      tap(() => {
        this.loading = true;
        this.skip = 0;
      }),
      switchMap((query) => this.productService.getProducts(query, this.limit, this.skip)),
      tap(() => this.loading = false)
    ).subscribe({
      next: (data: any) => {
        this.products = data?.products as ProductModel[];
        this.total = data?.total;
        console.log(this.total);
      },
      error: (err) => console.log(err)
    })
  }

  @HostListener('window:scroll', [])
  onScroll(): void {

    const scrollPosition = window.innerHeight + window.scrollY;
    const threshold = document.body.offsetHeight - 100;
    console.log(this.total, this.products.length);

    if (scrollPosition >= threshold && this.products.length < this.total) {
      this.loadMore();
    }
  }

  loadMore() {
    this.loading = true;
    this.skip += this.limit;
    this.productService.getProducts(this.searchQuery, this.limit, this.skip).subscribe({
      next: (data: any) => {
        this.products = [...this.products, ...data.products];
        this.loading = false;
      }
    })
  }

  scrollToTop(){
    window.scrollTo({top: 0, behavior: 'smooth'});
  }
}
