import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { User, UserForUpdate } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/users`;
  #userCache = new BehaviorSubject<User | null>(null);

  get userCache(): Observable<User | null> {
    return this.#userCache;
  }

  getUsers(
    filters: string = '',
    sortOrder: string = 'firstName',
    pageNumber: number = 1,
    pageSize: number = 20
  ): Observable<User[]> {
    return this.#http.get<User[]>(`${this.#baseUrl}`, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    });
  }

  getUserById(id: string): Observable<User> {
    return this.#http.get<User>(`${this.#baseUrl}/${id}`);
  }

  getCurrentUser(): Observable<User> {
    return this.#http
      .get<User>(`${this.#baseUrl}/me`)
      .pipe(
         tap(user => this.#userCache.next(user))
      );
  }

  updateCurrentUser(userForUpdate: UserForUpdate): Observable<User> {
    return this.#http.put<User>(`${this.#baseUrl}/me`, userForUpdate);
  }

  getCurrentUserComments(): Observable<any[]> {
    return this.#http.get<any[]>(`${this.#baseUrl}/me/comments`);
  }

  getCurrentUserRecipes(): Observable<any[]> {
    return this.#http.get<any[]>(`${this.#baseUrl}/me/recipes`);
  }

  getCurrentUserIngredients(): Observable<any[]> {
    return this.#http.get<any[]>(`${this.#baseUrl}/me/ingredients`);
  }

  getCurrentUserFavoriteRecipes(): Observable<any[]> {
    return this.#http.get<any[]>(`${this.#baseUrl}/me/favoriteRecipes`);
  }
}
