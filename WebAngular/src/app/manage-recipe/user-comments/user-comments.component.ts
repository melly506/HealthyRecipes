import { Component, OnInit, Input, ViewChild, ElementRef, DestroyRef, inject, signal, effect } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { MatTooltipModule } from '@angular/material/tooltip';
import { KEYCLOAK_EVENT_SIGNAL, KeycloakEventType, ReadyArgs, typeEventArgs } from 'keycloak-angular';

import { UserPictureComponent } from '../../shared/user-picture/user-picture.component';
import { ProgressLoaderComponent } from '../../shared/progress-loader/progress-loader.component';
import { CommentForManage, RecipeDetailed, User, UserComment } from '../../core/interfaces';
import { CommentsService, UsersService } from '../../core/services';
import { sbConfig, sbError } from '../../app.constant';
import { ConfirmDeleteModalComponent } from '../../shared/confirm-delete-modal/confirm-delete-modal.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-user-comments',
  templateUrl: './user-comments.component.html',
  styleUrls: ['./user-comments.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatSnackBarModule,
    UserPictureComponent,
    ProgressLoaderComponent
  ]
})
export class UserCommentsComponent implements OnInit {
  @Input() recipe!: RecipeDetailed;
  @ViewChild('commentInput') commentInput!: ElementRef;

  comments: UserComment[] = [];
  commentForm!: FormGroup;
  isLoading = false;
  isDeleting = false;
  isSubmitting = false;
  editingCommentId: string | null = null;
  authenticated = false;
  currentUser = signal<User | null>(null);

  #commentsService = inject(CommentsService);
  #usersService = inject(UsersService);
  #fb = inject(FormBuilder);
  #snackBar = inject(MatSnackBar);
  #dr = inject(DestroyRef);
  #dialog = inject(MatDialog);

  constructor() {
    const keycloakSignal = inject(KEYCLOAK_EVENT_SIGNAL);

    effect(async () => {
      const keycloakEvent = keycloakSignal();

      if (keycloakEvent.type === KeycloakEventType.Ready) {
        this.authenticated = typeEventArgs<ReadyArgs>(keycloakEvent.args);
        this.#loadCurrentUser();
      }

      if (keycloakEvent.type === KeycloakEventType.AuthLogout) {
        this.authenticated = false;
      }
    });
  }

  ngOnInit(): void {
    this.initForm();
    this.loadComments();
  }

  #loadCurrentUser(): void {
    if (!this.authenticated) {
      return;
    }
    this.#usersService.userCache
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe(user => {
        this.currentUser.set(user);
      });
  }

  private initForm(): void {
    this.commentForm = this.#fb.group({
      text: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(500)]]
    });
  }

  loadComments(): void {
    if (!this.recipe?.id) return;

    this.isLoading = true;
    this.#commentsService.getRecipeComments(this.recipe.id)
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe({
        next: (comments) => {
          this.comments = comments;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Помилка завантаження коментарів:', error);
          this.isLoading = false;
          this.#snackBar.open('Не вдалося завантажити коментарі', '', sbError);
        }
      });
  }

  submitComment(): void {
    this.commentForm.markAllAsTouched();
    if (this.commentForm.invalid || this.isSubmitting) {
      return;
    }

    const commentData: CommentForManage = {
      text: this.commentForm.value.text.trim()
    };

    this.isSubmitting = true;

    if (this.editingCommentId) {
      this.updateComment(commentData);
    } else {
      this.addComment(commentData);
    }
  }

  private addComment(commentData: CommentForManage): void {
    this.#commentsService.addCommentToRecipe(this.recipe.id, commentData)
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe({
        next: (newComment) => {
          this.comments.unshift(newComment);
          this.resetForm();
          this.isSubmitting = false;
          this.#snackBar.open('Коментар успішно додано', '', sbConfig);
        },
        error: (error) => {
          console.error('Помилка додавання коментаря:', error);
          this.isSubmitting = false;
          this.#snackBar.open('Не вдалося додати коментар', '', sbError);
        }
      });
  }

  private updateComment(commentData: CommentForManage): void {
    if (!this.editingCommentId) {
      return;
    }

    this.#commentsService.updateComment(this.editingCommentId, commentData)
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe({
        next: () => {
          const index = this.comments.findIndex(c => c.id === this.editingCommentId);
          if (index !== -1) {
            this.comments[index] = { ...this.comments[index], text: commentData.text, lastModifiedOn: new Date().toISOString() };
          }
          this.resetForm();
          this.isSubmitting = false;
          this.#snackBar.open('Коментар успішно оновлено', '', sbConfig);
        },
        error: (error) => {
          console.error('Помилка оновлення коментаря:', error);
          this.isSubmitting = false;
          this.#snackBar.open('Не вдалося оновити коментар', '', sbError);
        }
      });
  }

  editComment(comment: UserComment): void {
    this.editingCommentId = comment.id;
    this.commentForm.setValue({ text: comment.text });
    this.commentInput.nativeElement.focus();
  }

  cancelEdit(): void {
    this.resetForm();
  }

  deleteComment(commentId: string): void {
    const dialogRef = this.#dialog.open(ConfirmDeleteModalComponent, {
      width: '350px',
      data: {
        title: 'Видалити коментар',
        description: 'Ви впевнені, що хочете видалити цей коментар?'
      }
    });
    dialogRef.afterClosed()
      .pipe(
        takeUntilDestroyed(this.#dr)
      )
      .subscribe(result => {
        if (result === true) {
          this.isDeleting = true;
          this.#commentsService.deleteComment(commentId)
            .pipe(takeUntilDestroyed(this.#dr))
            .subscribe({
              next: () => {
                this.comments = this.comments.filter(c => c.id !== commentId);
                this.#snackBar.open('Коментар успішно видалено', '', sbConfig);
                this.isDeleting = false;
              },
              error: (error) => {
                console.error('Помилка видалення коментаря:', error);
                this.#snackBar.open('Не вдалося видалити коментар', '', sbError);
                this.isDeleting = false;
              }
            });
        }
      });
  }

  canModifyComment(comment: UserComment): boolean {
    // Поточний користувач - автор коментаря або поточний користувач - автор рецепта
    return (
      this.currentUser()?.identifier === comment.createdBy ||
      this.currentUser()?.identifier === this.recipe.createdBy
    );
  }

  private resetForm(): void {
    this.commentForm.reset({ text: '' });
    this.commentForm.get('text')?.markAsPristine();
    this.commentForm.get('text')?.setErrors(null);
    this.commentForm.get('text')?.markAsUntouched();
    this.editingCommentId = null;
  }

  getFormErrorMessage(): string {
    const control = this.commentForm.get('text');
    if (control?.hasError('required')) {
      return 'Текст коментаря обов\'язковий';
    }
    if (control?.hasError('minlength')) {
      return 'Коментар повинен містити мінімум 2 символи';
    }
    if (control?.hasError('maxlength')) {
      return 'Коментар не може перевищувати 500 символів';
    }
    return '';
  }

  // Метод для форматування дати
  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('uk-UA', {
      day: '2-digit',
      month: 'long',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}