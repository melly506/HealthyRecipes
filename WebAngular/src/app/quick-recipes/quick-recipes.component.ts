import { Component, inject, OnInit, signal } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatChip } from '@angular/material/chips';
import { MatIcon } from '@angular/material/icon';

import { RecipeSearchParams } from '../core/interfaces';
import { RecipeSearchComponent } from '../shared/recipe-search/recipe-search.component';
import { RecipesListComponent } from '../shared/recipes-list/recipes-list.component';

@Component({
  selector: 'app-quick-recipes',
  standalone: true,
  imports: [
    MatChip,
    MatIcon,
    RecipeSearchComponent,
    RecipesListComponent
  ],
  templateUrl: './quick-recipes.component.html',
  styleUrl: './quick-recipes.component.scss'
})
export class QuickRecipesComponent implements OnInit {
  #title = inject(Title);

  searchParamsSignal = signal<RecipeSearchParams>({
    searchTerm: '',
    foodType: null,
    season: null,
    diet: null,
    dishType: null
  });

  ngOnInit(): void {
    this.#title.setTitle('Green Spoon • Швидкі рецепти');
  }

  updateSearchParams(params: RecipeSearchParams): void {
    this.searchParamsSignal.set(params);
  }
}
