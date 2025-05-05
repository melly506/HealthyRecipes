import { Component, inject, Input, OnInit } from '@angular/core';
import { CookingMethod, Ingredient, RecipeIngredientDetails, StructuredNutritionInfo } from '../../core/interfaces';
import { NutritionCalculatorService } from './nutrition-calculator.service';
import { MatFormField, MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { DecimalPipe, PercentPipe } from '@angular/common';

import { cookingMethods } from './nutrition-calculator.constant';
import { InfoCardComponent } from '../info-card/info-card.component';

@Component({
  selector: 'app-nutrition-calculator',
  templateUrl: './nutrition-calculator.component.html',
  styleUrls: ['./nutrition-calculator.component.scss'],
  imports: [
    MatFormField,
    MatIconModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    InfoCardComponent,
    DecimalPipe,
    PercentPipe
  ],
  standalone: true
})
export class NutritionCalculatorComponent implements OnInit {
  // Вхідні дані: інгредієнти рецепту
  @Input() ingredients: Ingredient[] = [];
  @Input() counts: { [key: string]: number } = { };
  #nutritionCalculator = inject(NutritionCalculatorService);

  // Доступні методи приготування
  cookingMethods: CookingMethod[] = [...cookingMethods];

  // Обраний метод приготування
  selectedMethod: CookingMethod | null = null;

  // Інформація про харчову цінність
  nutritionInfo: StructuredNutritionInfo | null = null;

  ngOnInit(): void {
    // Встановлюємо перший метод приготування за замовчуванням
    this.selectedMethod = this.cookingMethods[0];

    // Розраховуємо початкові значення харчової цінності
    this.calculateNutrition();
  }

  // Обробник зміни методу приготування
  onCookingMethodChange(): void {
    this.calculateNutrition();
  }

  // Розрахунок харчової цінності страви
  calculateNutrition(): void {
    // Перевіряємо чи є інгредієнти для розрахунку
    if (!this.ingredients || this.ingredients.length === 0) {
      this.nutritionInfo = null;
      return;
    }

    const ingredientDetails: RecipeIngredientDetails[] = this.ingredients.map(ingredient => {
      return {
        ingredientId: ingredient.id,
        ingredientName: ingredient.name,
        carbs: ingredient.carbs,
        protein: ingredient.protein,
        sugar: ingredient.sugar,
        fat: ingredient.fat,
        calories: ingredient.calories,
        unit: ingredient.unit,
        count: this.counts[ingredient.id] || 0
      }
    });

    // Якщо обрано метод приготування, розраховуємо харчову цінність
    if (this.selectedMethod) {
      this.nutritionInfo = this.#nutritionCalculator.calculateFullNutrition(
        ingredientDetails,
        this.selectedMethod
      );
    } else {
      this.nutritionInfo = null;
    }
  }
}