import { Component } from '@angular/core';
import { Category, CategoryService } from '../../Services/category.Service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category-list',
  imports: [RouterLink, CommonModule],
  templateUrl: './category-list.html',
  styleUrl: './category-list.css'
})
export class CategoryList {
  categories: Category[] = [];
  page = 1;
  totalPages = 1;

  constructor(private service: CategoryService, private router: Router) { }

  ngOnInit() {
    this.fetchCategories();
  }

  fetchCategories() {
    this.service.getAll(this.page).subscribe(data => {
      this.categories = data;
    });
  }

  delete(id: number) {
    if (confirm('Are you sure you want to delete this?')) {
      this.service.delete(id).subscribe(() => this.fetchCategories());
    }
  }

  changePage(p: number) {
    this.page = p;
    this.fetchCategories();
  }

}
