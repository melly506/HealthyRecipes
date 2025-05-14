import { booleanAttribute, Component, DestroyRef, effect, inject, Input, OnInit } from '@angular/core';
import { MatFabButton } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import Keycloak from 'keycloak-js';
import { debounceTime, Subject } from 'rxjs';
import { KEYCLOAK_EVENT_SIGNAL, KeycloakEventType, ReadyArgs, typeEventArgs } from 'keycloak-angular';

import { RecipesService } from '../../../core/services';
import { ConfirmDeleteModalComponent } from '../../confirm-delete-modal/confirm-delete-modal.component';

@Component({
  selector: 'app-recipe-like',
  standalone: true,
  imports: [
    MatFabButton
  ],
  templateUrl: './recipe-like.component.html',
  styleUrl: './recipe-like.component.scss'
})
export class RecipeLikeComponent implements OnInit {
  @Input() recipeId: string = '';
  @Input({ transform: booleanAttribute }) isLiked: boolean = false;

  #keycloakService = inject(Keycloak);
  #dialog = inject(MatDialog);
  #recipesService = inject(RecipesService);
  #dr = inject(DestroyRef);
  #likeSubject = new Subject<boolean>();
  #animationDuration = 250;

  // For click animation
  isAnimating: boolean = false;
  // For click highlight effect
  isClicked: boolean = false;
  authenticated: boolean = false;

  constructor() {
    const keycloakSignal = inject(KEYCLOAK_EVENT_SIGNAL);

    effect(async () => {
      const keycloakEvent = keycloakSignal();

      if (keycloakEvent.type === KeycloakEventType.Ready) {
        this.authenticated = typeEventArgs<ReadyArgs>(keycloakEvent.args);
      }

      if (keycloakEvent.type === KeycloakEventType.AuthLogout) {
        this.authenticated = false;
      }
    });
  }

  ngOnInit(): void {
    this.#likeSubject.next(this.isLiked);
    this.#likeSubject.pipe(
      debounceTime(200), // Wait  before making API call
      takeUntilDestroyed(this.#dr)
    ).subscribe(liked => {
      if (liked) {
        this.#recipesService.likeRecipe(this.recipeId).subscribe();
      } else {
        this.#recipesService.unlikeRecipe(this.recipeId).subscribe();
      }
    });
  }

  toggleLike(event: MouseEvent): void {
    event.stopPropagation();
    event.preventDefault();
    if (!this.authenticated) {
      this.#requestAuth();
      return;
    }

    this.isLiked = !this.isLiked;

    // Trigger downscale animation
    this.isAnimating = true;
    setTimeout(() => {
      this.isAnimating = false;
    }, this.#animationDuration);

    // Trigger highlight effect
    this.isClicked = true;
    setTimeout(() => {
      this.isClicked = false;
    }, this.#animationDuration);

    this.#likeSubject.next(this.isLiked);
  }

  #requestAuth() {
    const dialogRef = this.#dialog.open(ConfirmDeleteModalComponent, {
      width: '350px',
      data: {
        title: 'Ви не авторизовані',
        description: 'Авторизуйтесь щоб мати можливіть зберегти рецепт'
      }
    });
    dialogRef.afterClosed()
      .pipe(
        takeUntilDestroyed(this.#dr)
      )
      .subscribe(async (result) => {
        if (result === true) {
          await this.#keycloakService.login();
        }
      });
  }
}
