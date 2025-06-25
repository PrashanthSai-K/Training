import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { RecipeModel } from '../models/recipe-model';
import { RecipeService } from '../services/recipe-service';
import { Recipe } from '../recipe/recipe';

@Component({
  selector: 'app-recipes',
  imports: [Recipe],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css'
})
export class Recipes implements OnInit {

  recipes: WritableSignal<RecipeModel[]> = signal<RecipeModel[]>([]);
  skip: number = 0;
  constructor(private recipeService: RecipeService) {
  }

  onNext() {
    if(this.skip < 30){
      this.skip += 9;
      this.getRecipes();
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  onPrev() {
    if (this.skip > 0) {
      this.skip -= 9;
      this.getRecipes();
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  getRecipes() {
    this.recipeService.getRecipes(this.skip).subscribe({
      next: (data) => {
        console.log(data);
        this.recipes.set(data?.recipes as RecipeModel[]);
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log("API call completed");
      }
    })
  }
  
  ngOnInit(): void {
    this.getRecipes()
  }
}
