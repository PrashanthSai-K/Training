import { Component } from '@angular/core';
import { Color, ColorService } from '../../Services/color.Service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-color-list',
  imports: [RouterLink, CommonModule],
  templateUrl: './color-list.html',
  styleUrl: './color-list.css'
})
export class ColorList {
  colors: Color[] = [];

  constructor(private colorService: ColorService, private router: Router) {}

  ngOnInit(): void {
    this.loadColors();
  }

  loadColors(): void {
    this.colorService.getAll().subscribe({
      next: (data) => {
        this.colors = data;
      }
    });
  }

  deleteColor(id: number): void {
    if (confirm("Are you sure you want to delete this color?")) {
      this.colorService.delete(id).subscribe(() => this.loadColors());
    }
  }
}
