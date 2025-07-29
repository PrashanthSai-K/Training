import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-product-create',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './product-create.html',
  styleUrl: './product-create.css'
})
export class ProductCreate implements OnInit {
  productForm!: FormGroup;
  categories: any[] = [];
  colors: any[] = [];
  models: any[] = [];
  users: any[] = [];

  constructor(private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit(): void {
    this.productForm = this.fb.group({
      productName: ['', Validators.required],
      image: ['', Validators.required],
      price: [null, Validators.required],
      categoryId: [null, Validators.required],
      colorId: [null, Validators.required],
      modelId: [null, Validators.required],
      userId: [null, Validators.required],
      sellStartDate: [null],
      sellEndDate: [null],
      isNew: [1]
    });

    this.loadDropdowns();
  }

  loadDropdowns() {
    this.http.get<any[]>('http://localhost:5038/api/category').subscribe(data => this.categories = data);
    this.http.get<any[]>('http://localhost:5038/api/colors').subscribe(data => this.colors = data);
    this.http.get<any[]>('http://localhost:5038/api/model').subscribe(data => this.models = data);
    this.http.get<any[]>('http://localhost:5038/api/user').subscribe(data => this.users = data);
  }

  onSubmit() {
    if (this.productForm.invalid) return;
    const startDateValue = this.productForm.get('sellStartDate')?.value;
    const endDateValue = this.productForm.get('sellEndDate')?.value;
    this.productForm.patchValue({
      sellStartDate : new Date(startDateValue).toISOString(),
      sellEndDate : new Date(endDateValue).toISOString(),
    });

    const formData = this.productForm.value;

    this.http.post('http://localhost:5038/api/products', formData).subscribe({
      next: () => alert('Product created successfully'),
      error: (err) => alert('Error creating product: ' + err.message)
    });
  }

  onDelete(productId: number) {
    if (!confirm('Are you sure you want to delete this product?')) return;

    this.http.delete(`http://localhost:5038/api/products/${productId}`).subscribe({
      next: () => alert('Product deleted successfully'),
      error: (err) => alert('Delete failed: ' + err.message)
    });
  }

  onEdit(productId: number) {
    this.http.get<any>(`http://localhost:5038/api/products/${productId}`).subscribe(product => {
      this.productForm.patchValue(product);
    });
  }
}
