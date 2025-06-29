import { Component, EventEmitter, inject, Output, signal } from '@angular/core';
import { CustomerModel } from '../../../core/models/register';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomerService } from '../../../core/services/customer-service';
import { PasswordValidator } from '../../../core/validators/password-validator';
import { CommonModule } from '@angular/common';
import { LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-create-customer',
  imports: [FormsModule, CommonModule, LucideAngularModule, ReactiveFormsModule],
  templateUrl: './create-customer.html',
  styleUrl: './create-customer.css'
})
export class CreateCustomer {
  isSubmitting = signal<boolean>(false);
  password = signal<string>("");
  showPassword = signal<boolean>(false);
  editingCustomer: CustomerModel | null = null;
  @Output() closeEdit = new EventEmitter<boolean>();

  customerForm = new FormGroup({
    name: new FormControl("", [Validators.required]),
    email: new FormControl("", [Validators.required, Validators.email]),
    phone: new FormControl("", [Validators.required, Validators.pattern(/^(0|[7-9])\d{9}$/)]),
  })

  public get name(): any {
    return this.customerForm.get("name")
  }

  public get email(): any {
    return this.customerForm.get("email")
  }

  public get phone(): any {
    return this.customerForm.get("phone")
  }


  private _snackbar = inject(MatSnackBar);

  constructor(private customerService: CustomerService) {
  }

  ngOnInit(): void {
    this.customerService.editingCustomer$.subscribe({
      next: (data) => {
        if (data) {
          this.editingCustomer = data;
          this.customerForm.setValue({ name: data.name, email: data.email, phone: data.phone });
          console.log(this.customerForm.value, "   ", data);
        }
      }
    })
  }

  generateRandomPassword(
    length: number = 8,
  ): string {
    const upperCase = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const lowerCase = 'abcdefghijklmnopqrstuvwxyz';
    const numbers = '0123456789';
    const symbols = '!@#$%^&';

    const getRandomChar = (chars: string) => chars[Math.floor(Math.random() * chars.length)];

    let password = [
      getRandomChar(upperCase),
      getRandomChar(lowerCase),
      getRandomChar(numbers),
      getRandomChar(symbols),
    ];

    const allChars = upperCase + lowerCase + numbers + symbols;
    for (let i = 4; i < length; i++) {
      password.push(getRandomChar(allChars));
    }

    password = password.sort(() => Math.random() - 0.5);

    return password.join('');
  }

  submitForm() {
    if (this.editingCustomer)
      this.onEditSubmit();
    else
      this.onCreateSubmit();
  }

  onCreateSubmit() {
    if (this.customerForm.invalid)
      return this._snackbar.open("Enter valid details", "", {
        duration: 1000
      })
    this.isSubmitting.set(true);
    this.password.set(this.generateRandomPassword());
    this.customerService.registerCustomer(this.customerForm.value as CustomerModel, this.password()).subscribe({
      next: (data) => {
        this.isSubmitting.set(false);
        this._snackbar.open("Customer has been created", "", {
          duration: 1000
        })
        this.showPassword.set(true);
        this.customerService.getCustomers().subscribe();
      },
      error: (err) => {
        console.log(err);
        this.isSubmitting.set(false);
        if (err?.error?.statusCode == 409) {
          this._snackbar.open(err.error.message, "", {
            duration: 1000
          })
        }
      }
    });
    return;
  }

  onEditSubmit() {
    if (this.customerForm.invalid)
      return this._snackbar.open("Enter valid details", "", {
        duration: 1000
      })
    this.isSubmitting.set(true);
    this.password.set(this.generateRandomPassword());
    if (this.editingCustomer)
      this.customerService.updateCustomer({ ...this.customerForm.value as CustomerModel, id: this.editingCustomer.id, status: this.editingCustomer.status }).subscribe({
        next: (data) => {
          this.isSubmitting.set(false);
          this._snackbar.open("Customer has been updated", "", {
            duration: 1000
          })
          this.customerService.getCustomers().subscribe();
          this.closeEdit.emit(true);
        },
        error: (err) => {
          console.log(err);
          this.isSubmitting.set(false);
          if (err?.error?.statusCode == 409) {
            this._snackbar.open(err.error.message, "", {
              duration: 1000
            })
          }
        }
      });
    return;
  }

  copyPasswordToClipboard() {
    navigator.clipboard.writeText(this.password());
    this._snackbar.open("Copied to clipboard", "", {
      duration: 1000
    })
  }

}
