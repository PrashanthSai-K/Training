import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';

import { SearchProducts } from './search-products';
import { ProductService } from '../service/product-service';
import { Products } from '../products/products';
import { FormsModule } from '@angular/forms';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { of } from 'rxjs';
import { CartItem } from '../models/cart-item';

describe('SearchProducts', () => {
  let component: SearchProducts;
  let fixture: ComponentFixture<SearchProducts>;
  let productServiceSpy: jasmine.SpyObj<ProductService>;

  const dummyProducts = {
    products: [
      { id: 1, title: 'Phone', thumbnail: "https://placehold.co/600x400" },
      { id: 2, title: 'Laptop', thumbnail: "https://placehold.co/600x400" }
    ],
    total: 20
  }

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('ProductService', ['getProductSerchResult', 'getAllProducts'])
    await TestBed.configureTestingModule({
      imports: [SearchProducts, Products, FormsModule],
      providers: [{ provide: ProductService, useValue: spy }],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(SearchProducts);
    component = fixture.componentInstance;
    productServiceSpy = TestBed.inject(ProductService) as jasmine.SpyObj<ProductService>;
    productServiceSpy.getAllProducts.and.returnValue(of(dummyProducts));
    productServiceSpy.getProductSerchResult.and.returnValue(of(dummyProducts));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should add item to cart', () => {
    component.handleAddToCart(1);
    expect(component.cartItems.length).toBe(1);
    expect(component.cartItems[0].Id).toBe(1);
    expect(component.cartItems[0].Count).toBe(1);
    expect(component.cartCount).toBe(1);
  });

  it('should increment an item in cart', () => {
    component.cartItems = [new CartItem(1, 1)];
    component.handleAddToCart(1);
    expect(component.cartItems.length).toBe(1);
    expect(component.cartItems[0].Id).toBe(1);
    expect(component.cartItems[0].Count).toBe(2);
    expect(component.cartCount).toBe(1);
  });

  it('search with debounce', fakeAsync(()=>{
    component.searchString = 'mobile';
    component.handleSearchProducts();

    tick(2000);
    fixture.detectChanges();

    expect(productServiceSpy.getProductSerchResult).toHaveBeenCalledWith('mobile');
    expect(component.products?.length).toBe(2);
  }));

});
