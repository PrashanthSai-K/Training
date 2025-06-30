import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { CustomerModel } from "../models/register";
import { BehaviorSubject, debounceTime, distinctUntilChanged, tap } from "rxjs";

@Injectable()
export class CustomerService {
    private custUrl = "http://localhost:5124/api/v1/customer";
    private httpClient = inject(HttpClient);
    search = "";

    private customerSubject = new BehaviorSubject<CustomerModel[]>([]);
    customer$ = this.customerSubject.asObservable();

    editingCustomerSubject = new BehaviorSubject<CustomerModel | null>(null);
    editingCustomer$ = this.editingCustomerSubject.asObservable();

    searchSubject = new BehaviorSubject<string>("");

    constructor() {
        this.searchSubject.pipe(
            debounceTime(500),
            distinctUntilChanged()
        ).subscribe({
            next: (data) => {
                this.search = data;
                this.getCustomers().subscribe();
            }
        })
    }

    registerCustomer(customer: CustomerModel, password: string | null = null) {
        if (password) {
            const customerForm = { ...customer, password: password };
            return this.httpClient.post<CustomerModel>(`${this.custUrl}`, customerForm);
        }
        else
            return this.httpClient.post<CustomerModel>(`${this.custUrl}`, customer);
    }

    getCustomers() {
        return this.httpClient.get<CustomerModel[]>(`${this.custUrl}`, {
            params: new HttpParams().set('query', this.search).set('pageSize', 1000)
        }).pipe(
            tap((data) => {
                this.customerSubject.next(data);
            })
        );
    }

    getCustomer() {
        return this.httpClient.get(`${this.custUrl}/profile`);
    }

    updateCustomer(customer: CustomerModel) {
        return this.httpClient.put(`${this.custUrl}/${customer.id}`, customer);
    }

    activateCustomer(id: number) {
        return this.httpClient.put(`${this.custUrl}/${id}/activate`, "");
    }

    deactivateCustomer(id: number) {
        return this.httpClient.put(`${this.custUrl}/${id}/deactivate`, "");
    }

}