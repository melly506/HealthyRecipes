import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Season } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class SeasonsService {
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/seasons`;

  getSeasons(
    filters: string = '',
    sortOrder: string = 'name',
    pageNumber: number = 1,
    pageSize: number = 25
  ): Observable<Season[]> {
    return this.#http.get<Season[]>(this.#baseUrl, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    });
  }
}