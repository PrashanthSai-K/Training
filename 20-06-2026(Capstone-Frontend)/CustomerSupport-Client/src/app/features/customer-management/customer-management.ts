import { Component, inject, OnInit, signal } from '@angular/core';
import { CustomerService } from '../../core/services/customer-service';
import { CustomerModel } from '../../core/models/register';
import { LucideAngularModule } from 'lucide-angular';
import { CreateCustomer } from "./create-customer/create-customer";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-customer-management',
  imports: [LucideAngularModule, CreateCustomer, CommonModule, FormsModule],
  templateUrl: './customer-management.html',
  styleUrl: './customer-management.css'
})
export class CustomerManagement implements OnInit {

  customers: CustomerModel[] = [];
  isCreateCustomerActive = signal<boolean>(false);
  isEditCustomerActive = signal<boolean>(false);
  searchQuery = "";

  constructor(private customerService: CustomerService) {
  }

  private _snackBar = inject(MatSnackBar);

  onSearch() {
    this.customerService.searchSubject.next(this.searchQuery);
  }

  activateCustomer(customer: CustomerModel) {
    this.customerService.activateCustomer(customer.id).subscribe({
      next: (data) => {
        this._snackBar.open("Customer account activated", "", {
          duration: 1000
        })
        this.customerService.getCustomers().subscribe();
        (document.activeElement as HTMLElement)?.blur(); 
      }
    })
  }

  deactivateCustomer(customer: CustomerModel) {
    this.customerService.deactivateCustomer(customer.id).subscribe({
      next: (data) => {
        this._snackBar.open("Customer account deactivated", "", {
          duration: 1000
        });
        this.customerService.getCustomers().subscribe();
        (document.activeElement as HTMLElement)?.blur(); 
      }
    })
  }

  ngOnInit(): void {
    this.customerService.getCustomers().subscribe({
      next: (data) => {
        this.customers = data;
      },
      error: (err) => {
        console.log(err);
      }
    });
    this.customerService.customer$.subscribe({
      next: (data) => {
        this.customers = data;
      }
    })
  }

  openCreateCustomer() {
    this.isCreateCustomerActive.set(true);
  }

  closeCustomerPopup() {
    this.isCreateCustomerActive.set(false);
    this.customerService.editingCustomerSubject.next(null);
    this.isEditCustomerActive.set(false);
  }

  closeEditPopup() {
    this.customerService.editingCustomerSubject.next(null);
    this.isEditCustomerActive.set(false);
  }

  openEditCustomer(customer: CustomerModel) {
    this.customerService.editingCustomerSubject.next(customer)
    this.isEditCustomerActive.set(true);
  }

}
