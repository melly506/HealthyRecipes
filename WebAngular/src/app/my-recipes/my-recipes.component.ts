import { Component, inject, OnInit, signal } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatChip } from '@angular/material/chips';

import { RecipeSource } from '../core/enums/recipe-source.enum';
import { RecipeSearchParams } from '../core/interfaces';
import { RecipeSearchComponent } from '../shared/recipe-search/recipe-search.component';
import { RecipesListComponent } from '../shared/recipes-list/recipes-list.component';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-my-recipes',
  standalone: true,
  imports: [
    RecipeSearchComponent,
    RecipesListComponent,
    MatChip,
    MatIcon
  ],
  templateUrl: './my-recipes.component.html',
  styleUrl: './my-recipes.component.scss'
})
export class MyRecipesComponent implements OnInit {
  #title = inject(Title);

  myRecipes: RecipeSource = RecipeSource.my;
  searchParamsSignal = signal<RecipeSearchParams>({
    searchTerm: '',
    foodType: null,
    season: null,
    diet: null,
    dishType: null
  });

  ngOnInit(): void {
    this.#title.setTitle('Green Spoon • Мої рецепти');
  }

  updateSearchParams(params: RecipeSearchParams): void {
    this.searchParamsSignal.set(params);
  }
}
