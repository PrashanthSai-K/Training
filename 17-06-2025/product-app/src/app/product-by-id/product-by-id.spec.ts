import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductById } from './product-by-id';

describe('ProductById', () => {
  let component: ProductById;
  let fixture: ComponentFixture<ProductById>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductById]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductById);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
