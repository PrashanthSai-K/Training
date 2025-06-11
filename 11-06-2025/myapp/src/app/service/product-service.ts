import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class ProductService {
    private httpClinet = inject(HttpClient);

    getProduct(id:number = 1) {
        return this.httpClinet.get(`https://dummyjson.com/products/${id}`);
    }

    getAllProducts():Observable<any>{
        return this.httpClinet.get(`https://dummyjson.com/products`);
    }
}