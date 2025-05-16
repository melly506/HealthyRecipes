import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { UserComment, CommentForManage } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {
  #http = inject(HttpClient);
  #baseUrl = `${environment.baseUrl}/${environment.apiVersion}/comments`;

  /**
   * Get a comment by its ID
   */
  getCommentById(commentId: string): Observable<UserComment> {
    return this.#http.get<UserComment>(`${this.#baseUrl}/${commentId}`);
  }

  /**
   * Update an existing comment
   */
  updateComment(commentId: string, comment: CommentForManage): Observable<CommentForManage> {
    return this.#http.put<CommentForManage>(`${this.#baseUrl}/${commentId}`, comment);
  }

  /**
   * Delete a comment
   */
  deleteComment(commentId: string): Observable<void> {
    return this.#http.delete<void>(`${this.#baseUrl}/${commentId}`);
  }

  /**
   * Add a comment to a recipe
   */
  addCommentToRecipe(recipeId: string, comment: CommentForManage): Observable<UserComment> {
    return this.#http.post<UserComment>(
      `${environment.baseUrl}/${environment.apiVersion}/recipes/${recipeId}/addComment`,
      comment
    );
  }

  /**
   * Get comments for a recipe with optional pagination and filtering
   */
  getRecipeComments(
    recipeId: string,
    filters: string = '',
    sortOrder: string = 'createdOn desc',
    pageNumber: number = 1,
    pageSize: number = 100
  ): Observable<UserComment[]> {
    return this.#http.get<UserComment[]>(
      `${environment.baseUrl}/${environment.apiVersion}/recipes/${recipeId}/comments`,
      {
        params: {
          filters,
          sortOrder,
          pageNumber,
          pageSize
        }
      }
    );
  }
}
