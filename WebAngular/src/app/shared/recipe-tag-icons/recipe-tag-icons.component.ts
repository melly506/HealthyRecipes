import { Component, Input, OnInit } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatTooltip } from '@angular/material/tooltip';

import { Diet, FoodType, RecipeDetailed } from '../../core/interfaces';

@Component({
  selector: 'app-recipe-tag-icons',
  standalone: true,
  imports: [
    MatIcon,
    MatTooltip
  ],
  templateUrl: './recipe-tag-icons.component.html',
  styleUrl: './recipe-tag-icons.component.scss'
})
export class RecipeTagIconsComponent implements OnInit {
  @Input() recipe?: RecipeDetailed;

  vegetarianism = '';
  diet5 = '';
  protein = '';
  diabetes = '';

  get hasItems(): boolean {
    return Boolean(this.diet5 || this.diabetes || this.vegetarianism || this.protein);
  }

  ngOnInit(): void {
    if (this.recipe) {
      this.diet5 = this.#getName<Diet>(this.recipe.diet, '5');
      this.diabetes = this.#getName<Diet>(this.recipe.diet, 'діабет');
      this.vegetarianism = this.#getName<FoodType>(this.recipe.foodType, 'вегетаріанство');
      this.protein = this.#getName<Diet>(this.recipe.diet, 'високобілкова');
    }
  }

  #getName<T extends { name: string }>(entities: T[], query: string) {
    const entity = entities.find(entity => entity.name.toLowerCase().includes(query.toLowerCase()));
    return entity?.name || '';
  }
}
