import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterOutlet } from '@angular/router';
import { CategoryList } from './Category/category-list/category-list';
import { Category, CategoryService } from './Services/category.Service';
import { CommonModule, TitleCasePipe } from '@angular/common';
import { ProductService } from './Services/product.Service';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, CommonModule, TitleCasePipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  categories: Category[] = [];
  breadCrumb:string="";
  year: number;

  protected title = 'ShopMigrationClient';
  constructor(private service: CategoryService, private productService: ProductService, private router: Router) { 
    this.year = new Date().getFullYear();
  }

  ngOnInit() {
    this.fetchCategories();
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe({
      next:(data)=>this.breadCrumb = data.urlAfterRedirects.split("/")[1]
    });
  }
  setCategories(id: number) {
    this.productService.category = id;
    this.productService.getProducts(1).subscribe();
  }
  fetchCategories() {
    this.service.getAll(1).subscribe(data => {
      this.categories = data;
    });
  }
}
