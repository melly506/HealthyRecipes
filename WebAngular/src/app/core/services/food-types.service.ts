import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { FoodType } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class FoodTypesService {
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/foodtypes`;

  getFoodTypes(
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 25
  ): Observable<FoodType[]> {
    return this.#http.get<FoodType[]>(this.#baseUrl, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    });
  }
}
