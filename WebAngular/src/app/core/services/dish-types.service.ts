import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { DishType } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class DishTypesService {
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/dishtypes`;

  getDishTypes(
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 25
  ): Observable<DishType[]> {
    return this.#http.get<DishType[]>(this.#baseUrl, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    });
  }
}
