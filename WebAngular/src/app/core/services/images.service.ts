import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ImageUploadResponse } from '../interfaces';


@Injectable({
  providedIn: 'root'
})
export class ImagesService {
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/images`;

  uploadProfileImage(file: File): Observable<ImageUploadResponse> {
    const formData = new FormData();
    formData.append('file', file);

    return this.#http.post<ImageUploadResponse>(`${this.#baseUrl}/uploadProfileImage`, formData);
  }

  uploadRecipeImage(file: File): Observable<ImageUploadResponse> {
    const formData = new FormData();
    formData.append('file', file);

    return this.#http.post<ImageUploadResponse>(`${this.#baseUrl}/uploadRecipeImage`, formData);
  }
}
