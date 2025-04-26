import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { debounceTime, distinctUntilChanged, fromEvent } from 'rxjs';

import { RecipesService } from '../../core/services';
import { RecipeDetailed } from '../../core/interfaces';
import { CookingTimeFormatPipe } from '../pipes/cooking-time-format.pipe';
import { RecipeTagIconsComponent } from '../recipe-tag-icons/recipe-tag-icons.component';
import { RouterLink } from '@angular/router';
import { ProgressLoaderComponent } from '../progress-loader/progress-loader.component';


@Component({
  selector: 'app-recipes-list',
  standalone: true,
  imports: [
    MatIcon,
    CookingTimeFormatPipe,
    RecipeTagIconsComponent,
    RouterLink,
    ProgressLoaderComponent
  ],
  templateUrl: './recipes-list.component.html',
  styleUrl: './recipes-list.component.scss'
})
export class RecipesListComponent implements OnInit {
  #recipesService = inject(RecipesService);
  #dr = inject(DestroyRef);

  recipes: RecipeDetailed[] = [];
  isLoading = false;
  noRecipesFound = false;

  currentPage = 1;
  pageSize = 20;
  hasMoreRecipes = true;

  filters = '';
  sortOrder = '';

  ngOnInit(): void {
    this.loadRecipes();
    this.#setupScrollListener();
  }

  loadRecipes(loadMore = false): void {
    if (!loadMore) {
      this.currentPage = 1;
      this.recipes = [];
      this.hasMoreRecipes = true;
    }

    if (!this.hasMoreRecipes) {
      return;
    }

    this.isLoading = true;

    this.#recipesService.getRecipes(
      this.filters,
      this.sortOrder,
      this.currentPage,
      this.pageSize
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
            }
          } else {
            this.noRecipesFound = false;
            this.recipes = [...this.recipes, ...recipes];
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
            this.loadRecipes(true);
          }
        }
      });
  }
}
