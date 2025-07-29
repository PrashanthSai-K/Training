import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ColorService } from '../../Services/color.Service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-color-edit',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './color-edit.html',
  styleUrl: './color-edit.css'
})
export class ColorEdit {
  form: FormGroup;
  colorId!: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private colorService: ColorService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      color1: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.colorId = +this.route.snapshot.params['id'];
    this.colorService.getById(this.colorId).subscribe(data => {
      this.form.patchValue(data);
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.colorService.update(this.colorId, this.form.value).subscribe(() => {
        this.router.navigate(['/colors']);
      });
    }
  }

}
