import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatError, MatFormField, MatInput, MatLabel } from '@angular/material/input';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSelect } from '@angular/material/select';
import { MatOption } from '@angular/material/core';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { catchError, of, tap } from 'rxjs';
import { NgxMaskDirective } from 'ngx-mask';

import { Ingredient, IngredientForCreation, ManageIngredientDialogData } from '../../../core/interfaces';
import { IngredientsService } from '../../../core/services';
import { ProgressLoaderComponent } from '../../progress-loader/progress-loader.component';
import { sbConfig, sbError } from '../../../app.constant';

@Component({
  selector: 'app-manage-ingredient',
  imports: [
    ProgressLoaderComponent,
    ReactiveFormsModule,
    MatFormField,
    MatInput,
    MatSelect,
    MatOption,
    MatIcon,
    MatLabel,
    MatButton,
    MatError,
    NgxMaskDirective
  ],
  standalone: true,
  templateUrl: './manage-ingredient.component.html',
  styleUrl: './manage-ingredient.component.scss'
})
export class ManageIngredientComponent {

  ingredientForm!: FormGroup;
  isSaving = false;
  isLoading = false;
  editMode = false;
  currentIngredient: Ingredient | null = null;

  #fb = inject(FormBuilder);
  #ingredientsService = inject(IngredientsService);
  #snackBar = inject(MatSnackBar);
  #dialogRef = inject<MatDialogRef<ManageIngredientComponent>>(MatDialogRef);
  #data = inject<ManageIngredientDialogData>(MAT_DIALOG_DATA);

  ngOnInit(): void {
    this.initForm();

    if (this.#data.ingredientId) {
      this.editMode = true;
      this.loadIngredient(this.#data.ingredientId);
    }
  }

  initForm(): void {
    this.ingredientForm = this.#fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      calories: [null, [Validators.required, Validators.min(0), Validators.max(900)]],
      unit: ['g', Validators.required],
      fat: [null, [Validators.required, Validators.min(0), Validators.max(100)]],
      carbs: [null, [Validators.required, Validators.min(0), Validators.max(100)]],
      protein: [null, [Validators.required, Validators.min(0), Validators.max(100)]],
      sugar: [null, [Validators.required, Validators.min(0), Validators.max(100)]]
    });
  }

  loadIngredient(id: string): void {
    this.isLoading = true;

    this.#ingredientsService.getIngredientById(id)
      .pipe(
        tap(ingredient => {
          this.currentIngredient = ingredient;
          this.ingredientForm.patchValue(ingredient);
          this.isLoading = false;
        }),
        catchError(error => {
          console.error(error);
          this.#snackBar.open('Помилка завантаження інгредієнта','', sbError);
          this.isLoading = false;
          return of(null);
        })
      )
      .subscribe();
  }

  saveIngredient(): void {
    if (this.ingredientForm.invalid) {
      this.ingredientForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const ingredientData: IngredientForCreation = this.ingredientForm.value;

    const saveOperation = this.editMode && this.#data.ingredientId
      ? this.#ingredientsService.updateIngredient(this.#data.ingredientId, ingredientData)
      : this.#ingredientsService.createIngredient(ingredientData);

    saveOperation
      .pipe(
        tap(response => {
          this.#snackBar.open(
            this.editMode ? 'Інгредієнт успішно оновлено' : 'Інгредієнт успішно створено',
            '',
            sbConfig
          );
          this.isSaving = false;
          const finalIngredient =  this.editMode && this.#data.ingredientId
            ? {
              id: this.#data.ingredientId,
              ...ingredientData
            }
            : response;
          this.#dialogRef.close(finalIngredient);
        }),
        catchError(error => {
          console.error(error);
          this.#snackBar.open('Помилка збереження інгредієнта', '', sbError);
          this.isSaving = false;
          return of(null);
        })
      )
      .subscribe();
  }

  cancel(): void {
    this.#dialogRef.close();
  }

  // Помічники для отримання контролів форми
  get nameControl() { return this.ingredientForm.get('name'); }
  get caloriesControl() { return this.ingredientForm.get('calories'); }
  get unitControl() { return this.ingredientForm.get('unit'); }
  get fatControl() { return this.ingredientForm.get('fat'); }
  get carbsControl() { return this.ingredientForm.get('carbs'); }
  get proteinControl() { return this.ingredientForm.get('protein'); }
  get sugarControl() { return this.ingredientForm.get('sugar'); }
}
