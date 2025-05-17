import { Component, DestroyRef, effect, inject, OnInit, signal } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabChangeEvent, MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, mergeMap, Observable, of, switchMap, tap } from 'rxjs';

import { KEYCLOAK_EVENT_SIGNAL, KeycloakEventType, ReadyArgs, typeEventArgs } from 'keycloak-angular';
import { UsersService } from '../core/services';
import { RecipeSearchParams, User, UserForUpdate } from '../core/interfaces';
import { UserPictureComponent } from '../shared/user-picture/user-picture.component';
import { GenderPipe } from '../shared/pipes/gender.pipe';
import { ProgressLoaderComponent } from '../shared/progress-loader/progress-loader.component';
import { UnauthorizedComponent } from '../shared/unauthorized/unauthorized.component';
import { projectName, sbConfig, sbError } from '../app.constant';
import { RecipesListComponent } from '../shared/recipes-list/recipes-list.component';
import { RecipeSource } from '../core/enums/recipe-source.enum';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    GenderPipe,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    UserPictureComponent,
    ProgressLoaderComponent,
    UnauthorizedComponent,
    RecipesListComponent,
    RouterLink
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  #usersService = inject(UsersService);
  #snackBar = inject(MatSnackBar);
  #title = inject(Title);
  #route = inject(ActivatedRoute);

  #fb = inject(FormBuilder);
  #dr = inject(DestroyRef);

  authenticated = false;
  currentUser = signal<User | null>(null);
  editMode = signal<boolean>(false);
  myRecipeSearchParamsSignal = signal<RecipeSearchParams>({
    searchTerm: '',
    foodType: null,
    season: null,
    diet: null,
    dishType: null
  });
  favoriteRecipeSearchParamsSignal = signal<RecipeSearchParams>({
    searchTerm: '',
    foodType: null,
    season: null,
    diet: null,
    dishType: null
  });
  myRecipes: RecipeSource = RecipeSource.my;
  likedRecipes: RecipeSource = RecipeSource.favorite;
  currentUserLoading = false;
  userForm!: FormGroup;
  isSaving = false;
  userId: string = '';

  constructor() {
    const keycloakSignal = inject(KEYCLOAK_EVENT_SIGNAL);


    effect(async () => {
      const keycloakEvent = keycloakSignal();

      if (keycloakEvent.type === KeycloakEventType.Ready) {
        this.authenticated = typeEventArgs<ReadyArgs>(keycloakEvent.args);
        this.#handleRouter();
      }

      if (keycloakEvent.type === KeycloakEventType.AuthLogout) {
        this.authenticated = false;
      }
    });
  }

  ngOnInit(): void {
    this.#title.setTitle(`${projectName} • Профіль`);
    this.initForm();
  }

  #loadCurrentUser(): Observable<User | null> {
    if (!this.authenticated) {
      return of(null);
    }
    this.currentUserLoading = true;
    return this.#usersService.getCurrentUser()
      .pipe(
        tap(user => {
          this.currentUser.set(user);
          this.currentUserLoading = false;
          this.updateForm(user);
        }),
        catchError(() => {
          this.currentUserLoading = false;
          this.#snackBar.open('Помилка завантаження користувача', '', sbError);
          return of(null);
        }),
        takeUntilDestroyed(this.#dr)
      );
  }

  #loadUserById(userId: string): Observable<User | null> {
    this.currentUserLoading = true;
    return this.#usersService.getUserById(userId)
      .pipe(
        tap(user => {
          this.currentUserLoading = false;
          this.currentUser.set(user);
          this.userId = user.id;
          this.updateForm(user);
        }),
        catchError(() => {
          this.currentUserLoading = false;
          this.#snackBar.open('Помилка завантаження користувача', '', sbError);
          return of(null);
        }),
        takeUntilDestroyed(this.#dr)
      );
  }

  initForm(): void {
    this.userForm = this.#fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(25)]],
      lastName: ['', [Validators.required, Validators.maxLength(30)]],
      email: [{ value: '', disabled: true }],
      username: [{ value: '', disabled: true }],
      bio:  ['', [Validators.maxLength(2000)]],
      gender: ['']
    });
  }

  updateForm(user: User): void {
    this.userForm.patchValue({
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
      username: user.username,
      bio: user.bio,
      gender: user.gender
    });
  }

  toggleEditMode(): void {
    if (this.editMode()) {
      // If we're exiting edit mode without saving, reset form
      this.updateForm(this.currentUser()!);
    }
    this.editMode.set(!this.editMode());
  }

  updateProfilePicture(pictureUrl: string): void {
    // Обновляем модель пользователя
    if (this.currentUser()) {
      const updatedUser = { ...this.currentUser()!, picture: pictureUrl };
      this.currentUser.set(updatedUser);

      // Обновляем информацию на сервере
      const userForUpdate: UserForUpdate = {
        firstName: this.userForm.get('firstName')?.value,
        lastName: this.userForm.get('lastName')?.value,
        bio: this.userForm.get('bio')?.value,
        gender: this.userForm.get('gender')?.value,
        picture: pictureUrl
      };

      this.#usersService.updateCurrentUser(userForUpdate)
        .pipe(
          mergeMap(() => this.#loadCurrentUser()),
          takeUntilDestroyed(this.#dr)
        )
        .subscribe({
          next: () => {
            this.#snackBar.open('Ваш аватар успішно оновлено', '', sbConfig);
          },
          error: () => {
            this.#snackBar.open('Помилка оновлення аватара', '', sbError);
          }
        });
    }
  }

  saveProfile(): void {
    // Mark all fields as touched to trigger validations
    this.userForm.markAllAsTouched();

    if (this.userForm.valid) {
      const userForUpdate: UserForUpdate = {
        firstName: this.userForm.get('firstName')?.value,
        lastName: this.userForm.get('lastName')?.value,
        bio: this.userForm.get('bio')?.value,
        gender: this.userForm.get('gender')?.value,
        picture: this.currentUser()?.picture || ''
      };

      this.isSaving = true;
      this.#usersService.updateCurrentUser(userForUpdate)
        .pipe(
          mergeMap(() => this.#loadCurrentUser()),
          takeUntilDestroyed(this.#dr)
        )
        .subscribe({
          next: () => {
            this.isSaving = false;
            this.#snackBar.open('Ваш профіль успішно оновлено', '', sbConfig);
            this.editMode.set(false);
          },
          error: () => {
            this.isSaving = false;
            this.#snackBar.open('Помилка оновлення користувача', '', sbError);
          }
        });
    }
  }

  onTabChange(event: MatTabChangeEvent) {
    switch (event.index) {
      case 0:
        this.myRecipeSearchParamsSignal.set({
          ...this.favoriteRecipeSearchParamsSignal(),
          searchTerm: ''
        });
        break
      case 1:
        this.favoriteRecipeSearchParamsSignal.set({
          ...this.favoriteRecipeSearchParamsSignal(),
          searchTerm: ''
        });
        break;
    }
  }

  #handleRouter() {
    this.#route.paramMap
      .pipe(
        switchMap(params => {
          const id = params.get('id');
          if (id) {
            return this.#loadUserById(id);
          }
          return this.#loadCurrentUser();
        }),
        takeUntilDestroyed(this.#dr)
      )
      .subscribe();
  }
}
