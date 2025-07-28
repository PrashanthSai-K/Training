import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { CategoryList } from './Category/category-list/category-list';
import { Category, CategoryService } from './Services/category.Service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  categories: Category[] = [];

  protected title = 'ShopMigrationClient';
  constructor(private service: CategoryService) { }

  ngOnInit() {
    this.fetchCategories();
  }

  fetchCategories() {
    this.service.getAll(1).subscribe(data => {
      this.categories = data;
    });
  }
}
