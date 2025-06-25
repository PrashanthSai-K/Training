import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipes } from './recipes';
import { RecipeService } from '../services/recipe-service';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { of } from 'rxjs';
import { Recipe } from '../recipe/recipe';
import { RecipeModel } from '../models/recipe-model';

describe('Recipes', () => {
  let component: Recipes;
  let fixture: ComponentFixture<Recipes>;
  let recipeServiceSpy: jasmine.SpyObj<RecipeService>;

  const dummyRecipe = {
    "recipes": [
      {
        "id": 1,
        "name": "Classic Margherita Pizza",
        "ingredients": [
          "Pizza dough",
          "Tomato sauce",
          "Fresh mozzarella cheese",
          "Fresh basil leaves",
          "Olive oil",
          "Salt and pepper to taste"
        ],
        "cookTimeMinutes": 15,
        "cuisine": "Italian",
        "image": "https://cdn.dummyjson.com/recipe-images/1.webp",
      },
      {
        "id": 2,
        "name": "Vegetarian Stir-Fry",
        "ingredients": [
          "Tofu, cubed",
          "Broccoli florets",
          "Carrots, sliced",
          "Bell peppers, sliced",
          "Soy sauce",
          "Ginger, minced",
          "Garlic, minced",
          "Sesame oil",
          "Cooked rice for serving"
        ],
        "cookTimeMinutes": 20,
        "cuisine": "Asian",
        "image": "https://cdn.dummyjson.com/recipe-images/2.webp",
      }
    ],
    total: 20
  }

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('RecipeService', ['getRecipes']);

    await TestBed.configureTestingModule({
      imports: [Recipes],
      providers: [{ provide: RecipeService, useValue: spy }],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(Recipes);
    component = fixture.componentInstance;
    recipeServiceSpy = TestBed.inject(RecipeService) as jasmine.SpyObj<RecipeService>;
    recipeServiceSpy.getRecipes.and.returnValue(of(dummyRecipe));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should add new recipe and length should be incremented', () => {
    const newrecipe = new RecipeModel(
      2,
      "Vegetarian Stir-Fry",
      [
        "Tofu, cubed",
        "Broccoli florets",
        "Carrots, sliced",
        "Bell peppers, sliced",
        "Soy sauce",
        "Ginger, minced",
        "Garlic, minced",
        "Sesame oil",
        "Cooked rice for serving"
      ],
      20,
      "Asian",
      "https://cdn.dummyjson.com/recipe-images/2.webp",
    )
    component.recipes.set([...component.recipes(), newrecipe]);

    expect(component.recipes().length).toBe(3);
  });

  it('should move to next page', () => {
    component.onNext();
    expect(component.skip).toBe(9);
    component.onNext();
    expect(component.skip).toBe(18);
  });

  it('should not move next more than 4 pages', () => {
    component.onNext();
    component.onNext();
    component.onNext();
    component.onNext();
    component.onNext(); //extra calls
    component.onNext();
    expect(component.skip).toBe(36);
  });

  it('should move back after moved forward', ()=>{
    component.onNext();
    component.onNext();
    component.onPrev();
    expect(component.skip).toBe(9);
  })
});
