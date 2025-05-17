import { Component, DestroyRef, effect, inject, Input, input, OnInit } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { debounceTime, distinctUntilChanged, fromEvent } from 'rxjs';

import { RecipesService } from '../../core/services';
import { RecipeDetailed, RecipeSearchParams } from '../../core/interfaces';
import { CookingTimeFormatPipe } from '../pipes/cooking-time-format.pipe';
import { RecipeTagIconsComponent } from '../recipe-tag-icons/recipe-tag-icons.component';
import { ProgressLoaderComponent } from '../progress-loader/progress-loader.component';
import { RecipeLikeComponent } from './recipe-like/recipe-like.component';
import { RecipeSource } from '../../core/enums/recipe-source.enum';


@Component({
  selector: 'app-recipes-list',
  standalone: true,
  imports: [
    MatIcon,
    CookingTimeFormatPipe,
    RecipeTagIconsComponent,
    RouterLink,
    ProgressLoaderComponent,
    MatButton,
    RecipeLikeComponent
  ],
  templateUrl: './recipes-list.component.html',
  styleUrl: './recipes-list.component.scss'
})
export class RecipesListComponent implements OnInit {
  #recipesService = inject(RecipesService);
  #dr = inject(DestroyRef);

  @Input() source: RecipeSource = RecipeSource.default;
  @Input() hideNotFoundButton = false;

  recipes: RecipeDetailed[] = [];
  isLoading = false;
  noRecipesFound = false;

  searchParams  = input<RecipeSearchParams>({
    searchTerm: '',
    foodType: null,
    season: null,
    diet: null,
    dishType: null
  });

  currentPage = 1;
  pageSize = 12;
  hasMoreRecipes = true;

  sortOrder = '';

  constructor() {
    effect(() => {
      const params = this.searchParams();
      this.loadRecipes(params);
    });
  }

  ngOnInit(): void {
    this.#setupScrollListener();
  }

  loadRecipes(params: RecipeSearchParams, loadMore = false): void {
    if (!loadMore) {
      this.currentPage = 1;
      this.hasMoreRecipes = true;
    }

    if (!this.hasMoreRecipes) {
      return;
    }

    this.isLoading = true;

    this.#recipesService.getRecipes(
      this.source,
      params.searchTerm,
      this.sortOrder,
      this.currentPage,
      this.pageSize,
      params.foodType?.id || '',
      params.season?.id || '',
      params.diet?.id || '',
      params.dishType?.id || '',
    )
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe({
        next: recipes => {
          this.isLoading = false;
          if (recipes.length < this.pageSize) {
            this.hasMoreRecipes = false;
          }

          if (recipes.length === 0) {
            if (this.currentPage === 1) {
              this.noRecipesFound = true;
              this.recipes = [];
            }
          } else {
            this.noRecipesFound = false;
            const currentRecipes = loadMore ? [...this.recipes] : [];
            this.recipes = [...currentRecipes, ...recipes];
            this.currentPage++;
          }

        },
        error: (error) => {
          console.error('Error loading recipes:', error);
          this.isLoading = false;
        }
      });
  }

  #setupScrollListener(): void {
    fromEvent(window, 'scroll')
      .pipe(
        debounceTime(100),
        distinctUntilChanged(),
        takeUntilDestroyed(this.#dr)
      )
      .subscribe(() => {
        const windowHeight = window.innerHeight;
        const documentHeight = document.documentElement.scrollHeight;
        const scrollTop = window.scrollY || document.documentElement.scrollTop;

        // Load more when user scrolls to bottom (with 200px threshold)
        if (windowHeight + scrollTop >= documentHeight - 200) {
          if (!this.isLoading && this.hasMoreRecipes) {
            this.loadRecipes(this.searchParams(), true);
          }
        }
      });
  }
}
