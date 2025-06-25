import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { CustomerModel } from "../models/register";

@Injectable()
export class CustomerService {
    private custUrl = "https://h4t8wz09-5124.inc1.devtunnels.ms/api/v1/customer";
    private httpClient = inject(HttpClient);

    registerCustomer(customer: CustomerModel) {
        return this.httpClient.post<CustomerModel>(`${this.custUrl}`, customer);
    }
}