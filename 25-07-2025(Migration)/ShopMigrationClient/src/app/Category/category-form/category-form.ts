import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../Services/category.Service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category-form',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './category-form.html',
  styleUrl: './category-form.css'
})
export class CategoryForm {
  form!: FormGroup;
  isEdit = false;
  id!: number;

  constructor(
    private fb: FormBuilder,
    private service: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      categoryId: [0],
      name: ['', Validators.required]
    });

    this.id = +this.route.snapshot.paramMap.get('id')!;
    if (this.id) {
      this.isEdit = true;
      this.service.getById(this.id).subscribe(data => this.form.patchValue(data));
    }
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const cat = this.form.value;
    if (this.isEdit) {
      this.service.update(cat).subscribe(() => this.router.navigate(['/categories']));
    } else {
      this.service.create(cat).subscribe(() => this.router.navigate(['/categories']));
    }
  }

}
