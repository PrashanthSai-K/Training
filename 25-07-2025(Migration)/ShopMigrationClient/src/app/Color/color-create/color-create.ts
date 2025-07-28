import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ColorService } from '../../Services/color.Service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-color-create',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './color-create.html',
  styleUrl: './color-create.css'
})
export class ColorCreate {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private colorService: ColorService,
    private router: Router
  ) {
    this.form = this.fb.group({
      color1: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.colorService.create(this.form.value).subscribe({
        next:(res) => {
        this.router.navigate(['/colors']);
      },
      error:(err)=>{
        console.log(err);
        
      }
    });
    }
  }

}
