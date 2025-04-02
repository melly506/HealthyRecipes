import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
// name services 
export class IngredientsService {
  //#- ПРИВАТНИЙ МЕТОД
  #http = inject(HttpClient);

  getIngredients(
    filters: string = '', 
    sortOrder: string = 'name', 
    pageNumber: number = 1,
    pageSize: number = 25
  ): Observable<any[]> {
    return this.#http.get<any[]>(`${environment.baseUrl}ingredients`, {
      params: {
        filters,
        sortOrder,
        pageNumber,
        pageSize
      }
    })
  }
}
