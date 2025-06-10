import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-customer-details',
  imports: [CommonModule],
  templateUrl: './customer-details.html',
  styleUrl: './customer-details.css'
})
export class CustomerDetails {
  customers: Customer[] = [
    { name: "sai", email: "sai@gmail.com", likes: 0, dislikes: 0 },
    { name: "gurur", email: "guru@gmail.com", likes: 0, dislikes: 0 },
    { name: "kavin", email: "kavin@gmail.com", likes: 0, dislikes: 0 }
  ]
  onLikeClick(email: string) {
    let customer = this.customers.find(c => c.email == email);
    if (customer)
      customer.likes++;
  }
  onDislikeClick(email: string) {
    let customer = this.customers.find(c => c.email == email);
    if (customer)
      customer.dislikes++;
  }

}

interface Customer {
  name: string;
  email: string;
  likes: number;
  dislikes: number;
}

