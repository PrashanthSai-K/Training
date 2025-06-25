import { Component, Input } from '@angular/core';
import { RecipeModel } from '../models/recipe-model';

@Component({
  selector: 'app-recipe',
  imports: [],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css'
})
export class Recipe {
  @Input() recipe:RecipeModel|undefined = undefined;
}
