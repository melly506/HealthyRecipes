import { Component, DestroyRef, effect, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, mergeMap, Observable, of, tap } from 'rxjs';

import { KEYCLOAK_EVENT_SIGNAL, KeycloakEventType, ReadyArgs, typeEventArgs } from 'keycloak-angular';
import { UsersService } from '../core/services';
import { User, UserForUpdate } from '../core/interfaces';
import { UserPictureComponent } from '../shared/user-picture/user-picture.component';
import { GenderPipe } from '../shared/pipes/gender.pipe';
import { ProgressLoaderComponent } from '../shared/progress-loader/progress-loader.component';
import { UnauthorizedComponent } from '../shared/unauthorized/unauthorized.component';
import { sbConfig, sbError } from '../app.constant';

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
    UnauthorizedComponent
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  #usersService = inject(UsersService);
  #snackBar = inject(MatSnackBar);

  #fb = inject(FormBuilder);
  #dr = inject(DestroyRef);

  authenticated = false;
  currentUser = signal<User | null>(null);
  editMode = signal<boolean>(false);
  currentUserLoading = signal<boolean>(false);
  userForm!: FormGroup;
  isSaving = false;

  constructor() {
    const keycloakSignal = inject(KEYCLOAK_EVENT_SIGNAL);


    effect(async () => {
      const keycloakEvent = keycloakSignal();

      if (keycloakEvent.type === KeycloakEventType.Ready) {
        this.authenticated = typeEventArgs<ReadyArgs>(keycloakEvent.args);
        if (this.authenticated) {
          this.#loadCurrentUser()
            .pipe(takeUntilDestroyed(this.#dr))
            .subscribe();
        }
      }

      if (keycloakEvent.type === KeycloakEventType.AuthLogout) {
        this.authenticated = false;
      }
    });
  }

  ngOnInit(): void {
    this.initForm();
  }

  #loadCurrentUser(): Observable<User | null> {
    if (!this.authenticated) {
      return of(null);
    }
    this.currentUserLoading.set(true);
    return this.#usersService.getCurrentUser()
      .pipe(
        tap(user => {
          this.currentUser.set(user);
          this.currentUserLoading.set(false);
          this.updateForm(user);
        }),
        catchError(error => {
          this.currentUserLoading.set(false);
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
          error: (error) => {
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
}
