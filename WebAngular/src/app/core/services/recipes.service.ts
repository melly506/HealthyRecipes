import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { RecipeDetailed, RecipeForUpdate, RecipeResponse } from '../interfaces';
import { RecipeSource } from '../enums/recipe-source.enum';

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
    source: RecipeSource = RecipeSource.default,
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 20,
    foodTypeId: string = '',
    seasonId: string = '',
    dietId: string = '',
    dishTypeId: string = '',
  ): Observable<RecipeDetailed[]> {
    let url;
    switch (source) {
      case RecipeSource.my:
        url = `${environment.baseUrl}/${environment.apiVersion}/users/me/recipes`;
        break;
      case RecipeSource.favorite:
        url = `${environment.baseUrl}/${environment.apiVersion}/users/me/favoriteRecipes`
        break;
      default:
        url = this.#baseUrl;
    }

    return this.#http.get<RecipeDetailed[]>(url, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize,
        foodTypeId,
        seasonId,
        dietId,
        dishTypeId
      }
    });
  }

  getMyRecipes(
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 20,
    foodTypeId: string = '',
    seasonId: string = '',
    dietId: string = '',
    dishTypeId: string = '',
  ): Observable<RecipeDetailed[]> {
    return this.#http.get<RecipeDetailed[]>(`${environment.baseUrl}/${environment.apiVersion}/users/me/recipes`, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize,
        foodTypeId,
        seasonId,
        dietId,
        dishTypeId
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

  likeRecipe(recipeId: string): Observable<void> {
    return this.#http.post<void>(`${this.#baseUrl}/${recipeId}/like`, { });
  }

  unlikeRecipe(recipeId: string): Observable<void> {
    return this.#http.delete<void>(`${this.#baseUrl}/${recipeId}/unlike`, { });
  }
}
