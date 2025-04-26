import { Component, DestroyRef, effect, inject, OnInit, ViewChild } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatError, MatFormField, MatInput, MatLabel } from '@angular/material/input';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButton } from '@angular/material/button';
import { KEYCLOAK_EVENT_SIGNAL, KeycloakEventType, ReadyArgs, typeEventArgs } from 'keycloak-angular';
import { catchError, Observable, of, switchMap, tap } from 'rxjs';

import { RecipePictureComponent } from '../shared/recipe-picture/recipe-picture.component';
import { CookingTimePickerComponent } from '../shared/cooking-time-picker/cooking-time-picker.component';
import {
  ChipsAutocompleteMultipleComponent
} from '../shared/chips-autocomplete-multiple/chips-autocomplete-multiple.component';
import {
  DietsService,
  DishTypesService,
  FoodTypesService,
  RecipesService,
  SeasonsService,
  UsersService
} from '../core/services';
import {
  Diet,
  DishType,
  FoodType,
  Season,
  RecipeIngredientDetails,
  RecipeDetailed,
  RecipeForUpdate,
  User,
  RecipeResponse
} from '../core/interfaces';
import { ManageIngredientsComponent } from '../shared/manage-ingredients/manage-ingredients.component';
import { projectName, sbConfig, sbError } from '../app.constant';
import { ProgressLoaderComponent } from '../shared/progress-loader/progress-loader.component';
import { RecipeDetailsComponent } from './recipe-details/recipe-details.component';
import { UnauthorizedComponent } from '../shared/unauthorized/unauthorized.component';

@Component({
  selector: 'app-manage-recipe',
  standalone: true,
  imports: [
    RecipePictureComponent,
    FormsModule,
    MatError,
    MatFormField,
    MatInput,
    MatLabel,
    ReactiveFormsModule,
    CookingTimePickerComponent,
    CdkTextareaAutosize,
    ChipsAutocompleteMultipleComponent,
    MatButton,
    ManageIngredientsComponent,
    ProgressLoaderComponent,
    RecipeDetailsComponent,
    UnauthorizedComponent,
    NgTemplateOutlet
  ],
  templateUrl: './manage-recipe.component.html',
  styleUrl: './manage-recipe.component.scss'
})
export class ManageRecipeComponent implements OnInit {
  @ViewChild(ManageIngredientsComponent) ingredientsComponent!: ManageIngredientsComponent;
  #usersService = inject(UsersService);
  #snackBar = inject(MatSnackBar);
  #fb = inject(FormBuilder);
  #dr = inject(DestroyRef);
  #foodTypesService = inject(FoodTypesService);
  #seasonsService = inject(SeasonsService);
  #dietsService = inject(DietsService);
  #dishTypesService = inject(DishTypesService);
  #route = inject(ActivatedRoute);
  #router = inject(Router);
  #title = inject(Title);
  #recipesService = inject(RecipesService);

  authenticated = false;
  recipeForm!: FormGroup;
  foodTypes: FoodType[] = [];
  seasons: Season[] = [];
  diets: Diet[] = [];
  dishTypes: DishType[] = [];
  recipeId: string | null = null;
  isLoading = false;
  isSaving = false;
  pageTitle = '';
  user?: User | null;
  currentUserLoading = false;
  recipeResponse?: RecipeResponse;

  get ingredientsControl() {
    return this.recipeForm.get('ingredients');
  }

  get ingredientError(): string | null {
    const control = this.ingredientsControl;

    // Only show errors if control is touched and invalid
    if (!control?.touched || !control?.errors) {
      return null;
    }

    if (control?.hasError('noIngredientsSelected')) {
      return 'У рецепті має бути щонайменше один інгредієнт';
    }
    if (control?.hasError('emptyIngredientCount')) {
      return 'Щоб продовжити, заповніть кількість для кожного доданого інгредієнта';
    }
    if (control.invalid) {
      return 'Будь ласка, заповніть інгредієнти';
    }
    return null;
  }

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
    this.#initForm();
    this.#loadLookups();
    this.#handleRouter();
  }

  cancel(): void {
    this.#router.navigate(['/']);
  }

  save(): void {
    this.recipeForm.markAllAsTouched();
    if (this.ingredientsComponent) {
      this.ingredientsComponent.markAllAsTouched();
    }
    if (this.recipeForm.invalid) {
      this.#snackBar.open('Переконайтесь, що всі поля заповнено коректно', '', sbError);
      return;
    }

    const formValue = this.recipeForm.value;

    const recipeForSubmit: RecipeForUpdate = {
      name: formValue.name,
      imageUrl: formValue.imageUrl,
      cookingTime: formValue.cookingTime,
      description: formValue.description,
      instructions: formValue.instructions,
      recipeIngridientsAssign: formValue.ingredients.map((ingredient: RecipeIngredientDetails) => ({
        count: ingredient.count,
        ingridientId: ingredient.ingredientId
      })),
      foodTypeIds: formValue.foodTypeIds,
      dietIds: formValue.dietIds,
      seasonIds: formValue.seasonIds,
      dishTypeIds: formValue.dishTypeIds
    };

    this.isSaving = true;

    const saveAction = this.recipeId
      ? this.#recipesService.updateRecipe(this.recipeId, recipeForSubmit)
      : this.#recipesService.createRecipe(recipeForSubmit);

    saveAction
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe({
        next: () => {
          this.isSaving = false;
          const message = this.recipeId
            ? 'Рецепт успішно змінено'
            : 'Рецепт успішно створено';
          this.#snackBar.open(message, '', sbConfig);
          this.#router.navigate(['/']);
        },
        error: (error) => {
          this.isSaving = false;
          this.#snackBar.open('Рецепт успішно створений', '', sbError);
          console.error('Error saving recipe:', error);
        }
      });
  }

  #loadLookups(): void {
    // Load food types
    this.#foodTypesService.getFoodTypes()
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe(foodTypes => {
        this.foodTypes = foodTypes;
      });

    // Load seasons
    this.#seasonsService.getSeasons()
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe(seasons => {
        this.seasons = seasons;
      });

    // Load diets
    this.#dietsService.getDiets()
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe(diets => {
        this.diets = diets;
      });

    // Load dish types
    this.#dishTypesService.getDishTypes()
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe(dishTypes => {
        this.dishTypes = dishTypes;
      });
  }

  #initForm(): void {
    this.recipeForm = this.#fb.group({
      name: ['', [Validators.required, Validators.maxLength(55)]],
      cookingTime: [30, [Validators.required, Validators.min(1)]],
      description:  ['', [Validators.maxLength(1000 )]],
      imageUrl: ['', [Validators.required]],
      instructions: ['', [Validators.required, Validators.maxLength(8000 )]],
      foodTypeIds: [
        [],
        [Validators.required]
      ],
      seasonIds: [
        [],
        [Validators.required]
      ],
      dietIds: [
        [],
        [Validators.required]
      ],
      dishTypeIds: [
        [],
        [Validators.required]
      ],
      ingredients: [
        [],
        [Validators.required, this.#validateIngredients]
      ]
    });
  }

  #populateForm(recipe: RecipeDetailed, ingredients: RecipeIngredientDetails[]): void {
    this.recipeForm.patchValue({
      name: recipe.name,
      cookingTime: recipe.cookingTime,
      description: recipe.description,
      imageUrl: recipe.imageUrl,
      instructions: recipe.instructions,
      foodTypeIds: (recipe.foodType || []).map(foodType => foodType.id),
      seasonIds: (recipe.season || []).map(season => season.id),
      dietIds: (recipe.diet || []).map(diet => diet.id),
      dishTypeIds: (recipe.dishType || []).map(dishType => dishType.id),
      ingredients
    });
  }

  #handleRouter() {
    this.#route.paramMap
      .pipe(
        takeUntilDestroyed(this.#dr),
        switchMap(params => {
          const id = params.get('id');
          if (id) {
            this.recipeId = id;
            this.pageTitle = 'Редагування рецепта';
            this.#title.setTitle(`${projectName} • ${this.pageTitle}`);
            this.isLoading = true;
            return this.#recipesService.getRecipeById(id);
          }
          this.pageTitle = 'Створити рецепт';
          this.#title.setTitle(`${projectName} • ${this.pageTitle}`);
          return of(null);
        })
      )
      .subscribe({
        next: response => {
          if (response?.recipe) {
            this.recipeResponse = response;
            this.#populateForm(response.recipe, response?.ingridientsDetails || []);
          }
          this.isLoading = false;
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Error loading recipe:', error);
        }
      });
  }

  #validateIngredients(control: AbstractControl): ValidationErrors | null {
    const ingredients = control.value as RecipeIngredientDetails[];

    if (!ingredients || ingredients.length === 0) {
      return { required: true };
    }

    return null;
  }

  #loadCurrentUser(): Observable<User | null> {
    if (!this.authenticated) {
      return of(null);
    }
    this.currentUserLoading = true;
    return this.#usersService.userCache
      .pipe(
        tap(user => {
          this.user = user;
          this.currentUserLoading = false;
        }),
        catchError(error => {
          this.currentUserLoading = false;
          console.error(error);
          return of(null);
        }),
        takeUntilDestroyed(this.#dr)
      );
  }
}
