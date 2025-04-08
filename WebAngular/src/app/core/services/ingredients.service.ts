import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
// name services 
export class IngredientsService {
  //#- ПРИВАТНИЙ МЕТОД
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/ingredients`;

  getIngredients(
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 25
  ): Observable<any[]> {
    return this.#http.get<any[]>(`${this.#baseUrl}`, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    })
  }
}
