import {
  Component, DestroyRef,
  ElementRef, EventEmitter,
  HostListener,
  inject,
  OnDestroy,
  OnInit, Output, Signal,
  ViewChild
} from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { MatFormField, MatInput } from '@angular/material/input';
import { MatChip, MatChipRemove, MatChipSet } from '@angular/material/chips';
import { MatIcon } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { MatButton, MatFabButton } from '@angular/material/button';
import { combineLatest, debounceTime, skip, startWith } from 'rxjs';

import { Diet, DishType, FoodType, RecipeSearchParams, Season } from '../../core/interfaces';
import { DietsService, DishTypesService, FoodTypesService, SeasonsService } from '../../core/services';
import { HeaderService } from '../../core/services/header.service';

interface RecipeSearchFilter {
  control: FormControl;
  title: string;
  icon: string;
  items: Signal<Diet[] | DishType[] | FoodType[] | Season[]>;
}

@Component({
  selector: 'app-recipe-search',
  imports: [
    ReactiveFormsModule,
    MatInput,
    MatFormFieldModule,
    MatFormField,
    MatIcon,
    MatOption,
    MatSelect,
    MatChipSet,
    MatChip,
    MatChipRemove,
    MatButton,
    MatFabButton,
    NgTemplateOutlet
  ],
  standalone: true,
  templateUrl: './recipe-search.component.html',
  styleUrl: './recipe-search.component.scss'
})
export class RecipeSearchComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput') searchInput!: ElementRef<HTMLInputElement>;
  @Output() searchParamsChange = new EventEmitter<RecipeSearchParams>();

  #foodTypesService = inject(FoodTypesService);
  #seasonsService = inject(SeasonsService);
  #dietsService = inject(DietsService);
  #headerService = inject(HeaderService);
  #dishTypesService = inject(DishTypesService);
  #dr = inject(DestroyRef);

  searchTermControl = new FormControl<string>('');
  foodTypeControl = new FormControl<FoodType | null>(null);
  seasonControl = new FormControl<Season | null>(null);
  dietControl = new FormControl<Diet | null>(null);
  dishTypeControl = new FormControl<DishType | null>(null);
  isActive = false;
  foodTypes: FoodType[] = [];
  seasons: Season[] = [];
  diets: Diet[] = [];
  dishTypes: DishType[] = [];

  recipeSearchFilters: RecipeSearchFilter[] = [{
    title: 'Тип харчування',
    control: this.foodTypeControl,
    icon: 'ramen_dining_outlined',
    items: toSignal(this.#foodTypesService.getFoodTypes(), { initialValue: [] })
  }, {
    title: 'Сезон',
    control: this.seasonControl,
    icon: 'sunny_snowing_outlined',
    items: toSignal(this.#seasonsService.getSeasons(), { initialValue: [] })
  }, {
    title: 'Дієта',
    control: this.dietControl,
    icon: 'medical_information_outlined',
    items: toSignal(this.#dietsService.getDiets(), { initialValue: [] })
  }, {
    title: 'Тип страви',
    control: this.dishTypeControl,
    icon: 'chef_hat_outlined',
    items: toSignal(this.#dishTypesService.getDishTypes(), { initialValue: [] })
  }];

  get hasFilters(): boolean {
    return this.recipeSearchFilters
      .some(recipeSearchFilter => recipeSearchFilter.control.value);
  }

  @HostListener('document:click', ['$event'])
  clickOutside(event: MouseEvent): void {
    if (!this.isActive) {
      return;
    }
    const clickedElement = event.target as HTMLElement;
    const formField = this.searchInput?.nativeElement.closest('mat-form-field');
    const selectionPanel = document.querySelector('.search-filters-panel');
    const selectionOption = document.querySelector('.mat-mdc-select-panel');
    const overlay = document.querySelector('.cdk-overlay-container');

    if (
      !formField?.contains(clickedElement)
      && !selectionPanel?.contains(clickedElement)
      && !selectionOption?.contains(clickedElement)
      && !overlay?.contains(clickedElement)
    ) {
      this.closePanel();
    }
  }


  @HostListener('document:keydown.escape')
  onEscapeKey(): void {
    this.closePanel();
  }

  openPanel(): void {
    setTimeout(() => {
      this.isActive = true;
      this.#headerService.hideHeader();
    });
  }

  ngOnInit(): void {
    const searchTerm$ = this.searchTermControl.valueChanges.pipe(startWith(''));
    const foodType$ = this.foodTypeControl.valueChanges.pipe(startWith(null));
    const season$ = this.seasonControl.valueChanges.pipe(startWith(null));
    const diet$ = this.dietControl.valueChanges.pipe(startWith(null));
    const dishType$ = this.dishTypeControl.valueChanges.pipe(startWith(null));

    combineLatest([
      searchTerm$,
      foodType$,
      season$,
      diet$,
      dishType$
    ])
      .pipe(
        debounceTime(150),
        skip(1),
        takeUntilDestroyed(this.#dr)
      )
      .subscribe(([searchTerm, foodType, season, diet, dishType]) => {
        const params: RecipeSearchParams = {
          searchTerm: this.#getSearchFilter(searchTerm),
          foodType,
          season,
          diet,
          dishType
        };

        this.searchParamsChange.emit(params);
      });
  }

  ngOnDestroy(): void {
    this.#headerService.showHeader();
  }

  closePanel(): void {
    this.isActive = false;
    this.#headerService.showHeader();
  }

  clearSearchAndFilters() {
    this.searchTermControl.reset();
    this.recipeSearchFilters.forEach(recipeSearchFilter => recipeSearchFilter.control.reset());
    this.closePanel();
  }

  #getSearchFilter(term: string | null = ''): string {
    return term ? `name @=* "${term}"` : '';
  }
}
