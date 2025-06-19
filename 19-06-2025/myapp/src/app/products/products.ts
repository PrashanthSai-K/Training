import { Component, inject, OnInit } from '@angular/core';
import { ProductModel } from '../models/product';
import { HttpClient } from '@angular/common/http';
import { ProductService } from '../service/product-service';
import { Product } from '../product/product';

@Component({
  selector: 'app-products',
  imports: [Product],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit {
  
  products:ProductModel[]|undefined =undefined;

  constructor(private productService:ProductService) {
  }

  ngOnInit() :void {
    this.productService.getAllProducts().subscribe({
      next:(data:any) => {
        this.products = data.products as ProductModel[];
      },
      error:(err)=> {
        console.log(err);
      },
      complete: ()=>{
        console.log("API call completed");
      }
    })

  }

}
