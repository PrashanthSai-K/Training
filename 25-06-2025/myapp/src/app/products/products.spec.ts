import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Products } from './products';
import jasmine from 'jasmine';
import { ProductService } from '../service/product-service';

describe('Products', () => {
  let component: Products;
  let fixture: ComponentFixture<Products>;
  let productServiceSpy: jasmine.SpyObj<ProductService>;
  

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Products]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Products);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
