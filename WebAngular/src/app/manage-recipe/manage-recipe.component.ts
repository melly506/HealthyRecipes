import { Component, DestroyRef, effect, inject, OnInit, ViewChild } from '@angular/core';
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
import { MatError, MatFormField, MatInput, MatLabel } from '@angular/material/input';
import { NgIf } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButton } from '@angular/material/button';
import { KEYCLOAK_EVENT_SIGNAL, KeycloakEventType, ReadyArgs, typeEventArgs } from 'keycloak-angular';

import { UnauthorizedComponent } from '../shared/unauthorized/unauthorized.component';
import { RecipePictureComponent } from '../shared/recipe-picture/recipe-picture.component';
import { CookingTimePickerComponent } from '../shared/cooking-time-picker/cooking-time-picker.component';
import {
  ChipsAutocompleteMultipleComponent
} from '../shared/chips-autocomplete-multiple/chips-autocomplete-multiple.component';
import { DietsService, DishTypesService, FoodTypesService, SeasonsService } from '../core/services';
import { Diet, DishType, FoodType, Season } from '../core/interfaces';
import { ManageIngredientsComponent } from '../shared/manage-ingredients/manage-ingredients.component';
import { RecipeIngredientDetails } from '../core/interfaces/recipe-ingredient';
import { MatSnackBar } from '@angular/material/snack-bar';
import { sbError } from '../app.constant';

@Component({
  selector: 'app-manage-recipe',
  standalone: true,
  imports: [
    UnauthorizedComponent,
    RecipePictureComponent,
    FormsModule,
    MatError,
    MatFormField,
    MatInput,
    MatLabel,
    NgIf,
    ReactiveFormsModule,
    CookingTimePickerComponent,
    CdkTextareaAutosize,
    ChipsAutocompleteMultipleComponent,
    MatButton,
    ManageIngredientsComponent
  ],
  templateUrl: './manage-recipe.component.html',
  styleUrl: './manage-recipe.component.scss'
})
export class ManageRecipeComponent implements OnInit {
  @ViewChild(ManageIngredientsComponent) ingredientsComponent!: ManageIngredientsComponent;
  #snackBar = inject(MatSnackBar);
  #fb = inject(FormBuilder);
  #dr = inject(DestroyRef);
  #foodTypesService = inject(FoodTypesService);
  #seasonsService = inject(SeasonsService);
  #dietsService = inject(DietsService);
  #dishTypesService = inject(DishTypesService);

  authenticated = false;
  recipeForm!: FormGroup;
  foodTypes: FoodType[] = [];
  seasons: Season[] = [];
  diets: Diet[] = [];
  dishTypes: DishType[] = [];

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
      }

      if (keycloakEvent.type === KeycloakEventType.AuthLogout) {
        this.authenticated = false;
      }
    });
  }

  ngOnInit(): void {
    this.#initForm();
    this.#loadLookups();
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
      description:  ['', [Validators.maxLength(10000)]],
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

  updateRecipePicture(pictureUrl: string): void {

  }

  cancel(): void {

  }

  save(): void {
    console.log('Form', this.recipeForm.value);
    this.recipeForm.markAllAsTouched();
    if (this.ingredientsComponent) {
      this.ingredientsComponent.markAllAsTouched();
    }
    if (this.recipeForm.invalid) {
      this.#snackBar.open('Переконайтесь, що всі поля заповнено коректно', '', sbError);
      return;
    }
  }

  #validateIngredients(control: AbstractControl): ValidationErrors | null {
    const ingredients = control.value as RecipeIngredientDetails[];

    if (!ingredients || ingredients.length === 0) {
      return { required: true };
    }

    return null;
  }

}
