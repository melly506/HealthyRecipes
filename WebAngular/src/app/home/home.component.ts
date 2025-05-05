import { Component, inject, OnInit, signal } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { RecipesListComponent } from '../shared/recipes-list/recipes-list.component';
import { RecipeSearchComponent } from '../shared/recipe-search/recipe-search.component';
import { RecipeSearchParams } from '../core/interfaces';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    RecipesListComponent,
    RecipeSearchComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  #title = inject(Title);
  searchParamsSignal = signal<RecipeSearchParams>({
    searchTerm: '',
    foodType: null,
    season: null,
    diet: null,
    dishType: null
  });

  ngOnInit(): void {
    this.#title.setTitle('Green Spoon');
  }

  updateSearchParams(params: RecipeSearchParams): void {
    this.searchParamsSignal.set(params);
  }

}
