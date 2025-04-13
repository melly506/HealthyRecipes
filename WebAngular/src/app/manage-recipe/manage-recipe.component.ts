import { Component, DestroyRef, effect, inject, OnInit } from '@angular/core';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
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
    MatButton
  ],
  templateUrl: './manage-recipe.component.html',
  styleUrl: './manage-recipe.component.scss'
})
export class ManageRecipeComponent implements OnInit {
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
      name: ['', [Validators.required, Validators.maxLength(50)]],
      cookingTime: [30],
      description:  ['', [Validators.maxLength(10000)]],
      foodTypeIds: [['5e809b11-21db-427e-b3e1-8bd0dbc3a939']],
      seasonIds: [[]],
      dietIds: [[]],
      dishTypeIds: [[]]
    });
  }

  updateRecipePicture(pictureUrl: string): void {

  }

  cancel(): void {

  }

  save(): void {
    console.log(this.recipeForm.value);
  }

}
