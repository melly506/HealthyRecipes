import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Diet } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class DietsService {
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/diets`;

  getDiets(
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 25
  ): Observable<Diet[]> {
    return this.#http.get<Diet[]>(this.#baseUrl, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    });
  }
}