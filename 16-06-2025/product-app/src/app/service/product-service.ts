import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable()
export class ProductService {
    constructor(private httpClient: HttpClient){
    }

    getProducts(query:string, limit:number=16, skip:number=0){
        return this.httpClient.get(`https://dummyjson.com/products/search?q=${query}&limit=${limit}&skip=${skip}`);
    }
}