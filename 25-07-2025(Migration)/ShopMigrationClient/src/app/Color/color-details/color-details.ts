import { Component } from '@angular/core';
import { Color, ColorService } from '../../Services/color.Service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-color-details',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './color-details.html',
  styleUrl: './color-details.css'
})
export class ColorDetails {
  color!: Color;

  constructor(
    private route: ActivatedRoute,
    private colorService: ColorService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = +this.route.snapshot.params['id'];
    this.colorService.getById(id).subscribe(data => this.color = data);
  }

}
