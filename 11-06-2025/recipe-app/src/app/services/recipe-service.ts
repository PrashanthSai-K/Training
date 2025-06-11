import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class RecipeService {
    httpClient:HttpClient = inject(HttpClient);

    getRecipes(skip:number=0):Observable<any>{
        return this.httpClient.get(`https://dummyjson.com/recipes?limit=9&skip=${skip}`);
    }
}