import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { IngredientDetailed, IngredientForCreation } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class IngredientsService {
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/ingredients`;

  getIngredients(
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 25
  ): Observable<IngredientDetailed[]> {
    return this.#http.get<IngredientDetailed[]>(`${this.#baseUrl}`, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    })
  }

  getMyIngredients(
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 100
  ): Observable<IngredientDetailed[]> {

    return this.#http.get<IngredientDetailed[]>(`${environment.baseUrl}/${environment.apiVersion}/users/me/ingredients`, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    });
  }

  getIngredientById(id: string): Observable<IngredientDetailed> {
    return this.#http.get<IngredientDetailed>(`${this.#baseUrl}/${id}`);
  }

  createIngredient(ingredient: IngredientForCreation): Observable<IngredientDetailed> {
    return this.#http.post<IngredientDetailed>(`${this.#baseUrl}`, ingredient);
  }

  updateIngredient(id: string, ingredient: IngredientForCreation): Observable<IngredientDetailed> {
    return this.#http.put<IngredientDetailed>(`${this.#baseUrl}/${id}`, ingredient);
  }

  deleteIngredient(id: string): Observable<void> {
    return this.#http.delete<void>(`${this.#baseUrl}/${id}`);
  }
}
