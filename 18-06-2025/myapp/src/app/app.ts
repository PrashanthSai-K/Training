import { Component } from '@angular/core';
// import { RouterOutlet } from '@angular/router';
import { Product } from "./product/product";
import { Products } from "./products/products";
import { SearchProducts } from "./search-products/search-products";
import { Navbar } from "./navbar/navbar";
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [Navbar, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'myapp';
}
