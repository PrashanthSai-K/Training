import { Component } from '@angular/core';
import { Category, CategoryService } from '../../Services/category.Service';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-category-details',
  imports: [RouterLink],
  templateUrl: './category-details.html',
  styleUrl: './category-details.css'
})
export class CategoryDetails {
  category!: Category;

  constructor(private service: CategoryService, private route: ActivatedRoute) { }

  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id')!;
    this.service.getById(id).subscribe(data => this.category = data);
  }

}
