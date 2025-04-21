import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Recipe, RecipeForUpdate, RecipeResponse } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class RecipesService {
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/recipes`;

  /**
   * Get a paginated list of recipes with optional filters and sorting
   */
  getRecipes(
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 20
  ): Observable<Recipe[]> {
    return this.#http.get<Recipe[]>(this.#baseUrl, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    });
  }

  /**
   * Get a recipe by its ID
   */
  getRecipeById(recipeId: string): Observable<RecipeResponse> {
    return this.#http.get<RecipeResponse>(`${this.#baseUrl}/${recipeId}`);
  }

  /**
   * Create a new recipe
   */
  createRecipe(recipe: RecipeForUpdate): Observable<RecipeForUpdate> {
    return this.#http.post<RecipeForUpdate>(this.#baseUrl, recipe);
  }

  /**
   * Update an existing recipe
   */
  updateRecipe(recipeId: string, recipe: RecipeForUpdate): Observable<RecipeForUpdate> {
    return this.#http.put<RecipeForUpdate>(`${this.#baseUrl}/${recipeId}`, recipe);
  }

  /**
   * Delete a recipe
   */
  deleteRecipe(recipeId: string): Observable<void> {
    return this.#http.delete<void>(`${this.#baseUrl}/${recipeId}`);
  }
}
