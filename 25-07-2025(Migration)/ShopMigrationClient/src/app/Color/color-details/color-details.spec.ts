import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorDetails } from './color-details';

describe('ColorDetails', () => {
  let component: ColorDetails;
  let fixture: ComponentFixture<ColorDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ColorDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ColorDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
