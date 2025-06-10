import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {First} from "./first/first";
import { CustomerDetails } from "./customer-details/customer-details";
import { Cart } from "./cart/cart";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [First, CustomerDetails, Cart]
})
export class App {
  protected title = 'myapp';
}
