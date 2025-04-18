import {
  AfterViewInit, ChangeDetectorRef,
  Component,
  DestroyRef,
  ElementRef,
  forwardRef,
  HostListener,
  inject,
  OnInit,
  ViewChild
} from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatFormField, MatInput, MatPrefix, MatSuffix } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import {
  AbstractControl,
  ControlValueAccessor,
  FormControl,
  FormGroup,
  FormsModule, NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule, ValidationErrors,
  Validator,
  Validators
} from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatList, MatListItem } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { MatIconButton } from '@angular/material/button';
import { debounceTime, distinctUntilChanged, finalize, switchMap, tap } from 'rxjs';
import { NgxMaskDirective } from 'ngx-mask';

import { IngredientsService } from '../../core/services';
import { Ingredient } from '../../core/interfaces';
import { ProgressLoaderComponent } from '../progress-loader/progress-loader.component';
import { UnitPipe } from '../pipes/unit.pipe';
import { RecipeIngredientDetails } from '../../core/interfaces/recipe-ingredient';

@Component({
  selector: 'app-manage-ingredients',
  standalone: true,
  imports: [
    MatFormField,
    MatIcon,
    FormsModule,
    MatPrefix,
    MatSuffix,
    MatInput,
    MatFormFieldModule,
    ProgressLoaderComponent,
    ReactiveFormsModule,
    MatList,
    MatListItem,
    MatTableModule,
    MatSortModule,
    UnitPipe,
    MatIconButton,
    NgxMaskDirective
  ],
  templateUrl: './manage-ingredients.component.html',
  styleUrl: './manage-ingredients.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ManageIngredientsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => ManageIngredientsComponent),
      multi: true
    }
  ]
})
export class ManageIngredientsComponent implements OnInit, AfterViewInit, ControlValueAccessor, Validator {
  @ViewChild('searchInput') searchInput!: ElementRef<HTMLInputElement>;
  @ViewChild('ingredientsContainer') ingredientsContainer!: ElementRef<HTMLDivElement>;
  @ViewChild(MatSort) sort!: MatSort;

  #ingredientsService = inject(IngredientsService);
  #dr = inject(DestroyRef);
  #cd = inject(ChangeDetectorRef);
  isActive = false;

  searchTermControl = new FormControl<string>('');
  ingredientsForm: FormGroup = new FormGroup({});

  currentPage = 1;
  pageSize = 10;
  hasMoreItems = true;
  isLoading = false;

  filteredIngredients: Ingredient[] = [];
  selectedIngredients: Ingredient[] = [];
  ingredientCounts: Map<string, number | null> = new Map<string, number | null>();

  displayedColumns = ['name', 'calories', 'fat', 'protein', 'carbs', 'sugar', 'unit', 'actions'];

  onChange: any = () => {};
  onTouched: any = () => {};

  @HostListener('document:click', ['$event'])
  clickOutside(event: MouseEvent): void {
    if (!this.isActive) {
      return;
    }
    const clickedElement = event.target as HTMLElement;
    const formField = this.searchInput?.nativeElement.closest('mat-form-field');
    const selectionPanel = document.querySelector('.manage-recipe-selection');

    if (formField && !formField.contains(clickedElement) &&
      selectionPanel && !selectionPanel.contains(clickedElement)) {
      this.closePanel();
    }
  }

  @HostListener('document:keydown.escape')
  onEscapeKey(): void {
    this.closePanel();
  }

  ngOnInit(): void {
    this.searchTermControl.valueChanges.pipe(
      debounceTime(60),
      distinctUntilChanged(),
      tap(() => {
        this.currentPage = 1;
        this.hasMoreItems = true;
      }),
      switchMap(term => this.#ingredientsService.getIngredients(
        this.#getSearchFilter(term),
        'name',
        this.currentPage,
        this.pageSize
      )),
      takeUntilDestroyed(this.#dr)
    ).subscribe(ingredients => {
      this.filteredIngredients = ingredients;
      this.hasMoreItems = ingredients.length === this.pageSize;
    });

    this.searchTermControl.setValue('');

    this.ingredientsForm.valueChanges.pipe(
      takeUntilDestroyed(this.#dr)
    ).subscribe(() => {
      this.#updateFormValue();
    });
  }

  ngAfterViewInit() {
    this.sort.sortChange
      .pipe(
        debounceTime(20),
        takeUntilDestroyed(this.#dr)
      )
      .subscribe(sort => {
        this.#sortIngredients(sort);
      });
  }

  isAdded(ingredient: Ingredient): boolean {
    return !!this.selectedIngredients
      .find(selectedIngredient => selectedIngredient.id === ingredient.id);
  }

  selectItem(ingredient: Ingredient): void {
    this.closePanel();
    this.searchTermControl.setValue('');
    const existingIngredient = this.selectedIngredients.find(selectedIngredient => {
      return ingredient.id === selectedIngredient.id;
    });
    if (!existingIngredient) {
      this.selectedIngredients.push(ingredient);
      this.selectedIngredients = [...this.selectedIngredients];

      this.ingredientsForm.addControl(
        ingredient.id,
        new FormControl(null, [Validators.required, Validators.min(1)])
      );
      this.#updateFormValue();
    }
  }

  onScroll(): void {
    const element = this.ingredientsContainer.nativeElement;
    const scrollPosition = element.scrollTop + element.clientHeight;
    const scrollHeight = element.scrollHeight;

    if (scrollPosition >= scrollHeight * 0.8 && this.hasMoreItems && !this.isLoading) {
      this.loadMoreItems();
    }
  }

  loadMoreItems(): void {
    this.isLoading = true;
    this.currentPage++;

    this.#ingredientsService.getIngredients(
      this.#getSearchFilter(this.searchTermControl.value),
      'name',
      this.currentPage,
      this.pageSize
    ).pipe(
      finalize(() => this.isLoading = false),
      takeUntilDestroyed(this.#dr)
    ).subscribe(ingredients => {
      this.filteredIngredients = [...this.filteredIngredients, ...ingredients];
      this.hasMoreItems = ingredients.length === this.pageSize;
    });
  }

  openPanel(): void {
    setTimeout(() => {
      this.isActive = true;
    });
  }

  closePanel(): void {
    this.isActive = false;
  }

  removeIngredient(ingredient: Ingredient): void {
    this.selectedIngredients = this.selectedIngredients.filter(selectedIngredient => {
      return selectedIngredient.id !== ingredient.id;
    });
    this.ingredientCounts.delete(ingredient.id);
    if (this.ingredientsForm.get(ingredient.id)) {
      this.ingredientsForm.removeControl(ingredient.id);
    }
    this.#updateFormValue();
  }

  updateIngredientCount(ingredientId: string, event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const count = parseInt(inputElement.value, 10);

    if (!isNaN(count) && count > 0) {
      this.ingredientCounts.set(ingredientId, count);
      if (this.ingredientsForm.get(ingredientId)) {
        this.ingredientsForm.get(ingredientId)?.setValue(count);
      }
    } else if (inputElement.value === '') {
      // Handle empty input case
      this.ingredientCounts.set(ingredientId, 0);
      if (this.ingredientsForm.get(ingredientId)) {
        this.ingredientsForm.get(ingredientId)?.setValue(null);
      }
    }

    this.#updateFormValue();
  }

  writeValue(ingredientDetails: RecipeIngredientDetails[]): void {
    if (ingredientDetails && ingredientDetails.length) {
      // Clear existing data
      this.selectedIngredients = [];
      this.ingredientCounts.clear();
      this.ingredientsForm = new FormGroup({});

      for (const detail of ingredientDetails) {
        const ingredient = {
          id: detail.ingredientId,
          name: detail.ingredientName,
          calories: detail.calories,
          fat: detail.fat,
          protein: detail.protein,
          carbs: detail.carbs,
          sugar: detail.sugar,
          unit: detail.unit,
        } as Ingredient;

        this.selectedIngredients.push(ingredient);
        const count = detail.count || null;
        this.ingredientCounts.set(detail.ingredientId, count);
        this.ingredientsForm.addControl(
          detail.ingredientId,
          new FormControl(count, [Validators.required, Validators.min(1)])
        );
      }

      this.selectedIngredients = [...this.selectedIngredients];
      this.#cd.detectChanges();
    } else {
      // Clear all data when empty array is passed
      this.selectedIngredients = [];
      this.ingredientCounts.clear();
      this.ingredientsForm = new FormGroup({});
    }

    this.onChange(this.getCurrentValue());
  }

  getInputFormControl(row: Ingredient): FormControl {
    return this.ingredientsForm.get(row.id) as FormControl;
  }

  validate(control: AbstractControl): ValidationErrors | null {
    // Check if any ingredients are selected
    if (this.selectedIngredients.length === 0) {
      return { noIngredientsSelected: true };
    }

    // Check if there are any ingredients with no count
    const hasEmptyCounts = Array
      .from(this.ingredientCounts.values())
      .some(count => count === null || count <= 0);
    if (hasEmptyCounts) {
      return { emptyIngredientCount: true };
    }

    // Check if all ingredients have valid counts
    if (this.ingredientsForm.invalid) {
      return { invalidIngredientCount: true };
    }

    return null;
  }

  markAllAsTouched(): void {
    Object.keys(this.ingredientsForm.controls).forEach(key => {
      const control = this.ingredientsForm.get(key);
      control?.markAsTouched();
    });
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  getCurrentValue(): RecipeIngredientDetails[] {
    return this.selectedIngredients
      .map(ingredient => {
        return {
          ingredientId: ingredient.id,
          count: this.ingredientCounts.get(ingredient.id) || null,
          ingredientName: ingredient.name,
          calories: ingredient.calories,
          unit: ingredient.unit,
          fat: ingredient.fat,
          carbs: ingredient.carbs,
          protein: ingredient.protein,
          sugar: ingredient.sugar
        };
      });
  }

  #updateFormValue(): void {
    this.onChange(this.getCurrentValue());
  }

  #getSearchFilter(term: string | null = ''): string {
    return term ? `name @=* "${term}"` : '';
  }

  #sortIngredients(sort: Sort = { active: 'name', direction: 'desc' }): void {
    this.selectedIngredients = [...this.selectedIngredients].sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      const key = sort.active as keyof Ingredient;

      // Use the active key to directly access the property
      return this.#compare(a[key], b[key], isAsc);
    });
    this.#cd.detectChanges();
  }

  #compare(a: any, b: any, isAsc: boolean): number {
    // Handle string comparison
    if (typeof a === 'string' && typeof b === 'string') {
      return (a.localeCompare(b)) * (isAsc ? 1 : -1);
    }

    // Handle number comparison
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
}
