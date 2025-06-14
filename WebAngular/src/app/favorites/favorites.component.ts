import { Component, inject, OnInit, signal } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatChip } from '@angular/material/chips';
import { MatIcon } from '@angular/material/icon';

import { RecipeSearchComponent } from '../shared/recipe-search/recipe-search.component';
import { RecipesListComponent } from '../shared/recipes-list/recipes-list.component';
import { RecipeSearchParams } from '../core/interfaces';
import { RecipeSource } from '../core/enums/recipe-source.enum';

@Component({
  selector: 'app-favorites',
  standalone: true,
  imports: [
    RecipeSearchComponent,
    RecipesListComponent,
    MatChip,
    MatIcon
  ],
  templateUrl: './favorites.component.html',
  styleUrl: './favorites.component.scss'
})
export class FavoritesComponent implements OnInit {
  #title = inject(Title);

  likedRecipes: RecipeSource = RecipeSource.favorite;
  searchParamsSignal = signal<RecipeSearchParams>({
    searchTerm: '',
    foodType: null,
    season: null,
    diet: null,
    dishType: null
  });

  ngOnInit(): void {
    this.#title.setTitle('Green Spoon • Улюблені рецепти');
  }

  updateSearchParams(params: RecipeSearchParams): void {
    this.searchParamsSignal.set(params);
  }
}
