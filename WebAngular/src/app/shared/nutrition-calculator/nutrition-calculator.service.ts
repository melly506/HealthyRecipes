import { Injectable } from '@angular/core';
import {
  CookingMethod,
  RecipeIngredientDetails,
  NutritionInfo,
  NutritionPercentage,
  StructuredNutritionInfo
} from '../../core/interfaces';

@Injectable({
  providedIn: 'root'
})
export class NutritionCalculatorService {

  // Визначаємо коефіцієнт конвертації ml в gram
  #getDensityFactor(ingredient: RecipeIngredientDetails): number {
    const name = ingredient.ingredientName.toLowerCase();

    if (name.includes('олія')) {
      return 0.92;
    }
    else if (name.includes('сироп')) {
      return 1.33;
    }
    else if (name.includes('мед')) {
      return 1.42;
    }
    else if  (name.includes('вино')) {
      return 0.94;
    }
    return 1;
  }

  // Розрахунок маси окремого інгредієнта з урахуванням коефіцієнту щільності
  #calculateGramWeight(ingredient: RecipeIngredientDetails): number {
    const ingredientCount = ingredient.count || 0;
    if (ingredient.unit === 'g') {
      // Якщо одиниця виміру вже грам, то просто повертаємо значення
      return ingredientCount;
    } else if (ingredient.unit === 'ml') {
      // Якщо одиниця вимірювання — мілілітри, конвертуємо в грами
      // використовуючи коефіцієнт щільності
      return ingredientCount * this.#getDensityFactor(ingredient);
    }
    return ingredientCount; // Повертаємо як є у випадку невідомої одиниці
  }

  // Розрахунок відсоткового співвідношення білків, жирів та вуглеводів
  #calculateMacroPercentages(protein: number, fat: number, carbs: number): NutritionPercentage {
    // Обчислюємо загальну суму макроелементів
    const totalMacros = protein + fat + carbs;

    // Якщо загальна сума дорівнює 0, повертаємо нульові значення
    if (totalMacros === 0) {
      return {
        proteinEnergyPercentage: 0,
        fatEnergyPercentage: 0,
        carbsEnergyPercentage: 0
      };
    }

    // Розраховуємо відсотки від загальної суми
    return {
      proteinEnergyPercentage: protein / totalMacros,
      fatEnergyPercentage: fat / totalMacros,
      carbsEnergyPercentage: carbs / totalMacros
    };
  }

  // Розрахунок загальної маси інгредієнтів у грамах
  #calculateTotalWeight(ingredients: RecipeIngredientDetails[]): number {
    return ingredients.reduce((sum: number, ingredient: RecipeIngredientDetails) => {
      return sum + this.#calculateGramWeight(ingredient);
    }, 0);
  }

  // Розрахунок загальної кількості калорій для всіх інгредієнтів
  #calculateTotalCalories(ingredients: RecipeIngredientDetails[]): number {
    return ingredients.reduce((sum, ingredient) => {
      // Розраховуємо масу інгредієнта у грамах
      const weightInGrams = this.#calculateGramWeight(ingredient);
      // Множимо калорійність на 100г на частку від 100г
      return sum + (ingredient.calories * weightInGrams / 100);
    }, 0);
  }


  // Розрахунок загальної кількості білків для всіх інгредієнтів
  #calculateTotalProtein(ingredients: RecipeIngredientDetails[]): number {
    return ingredients.reduce((sum, ingredient) => {
      const weightInGrams = this.#calculateGramWeight(ingredient);
      return sum + (ingredient.protein * weightInGrams / 100);
    }, 0);
  }

  // Розрахунок загальної кількості жирів для всіх інгредієнтів
  #calculateTotalFat(ingredients: RecipeIngredientDetails[]): number {
    return ingredients.reduce((sum, ingredient) => {
      const weightInGrams = this.#calculateGramWeight(ingredient);
      return sum + (ingredient.fat * weightInGrams / 100);
    }, 0);
  }

  // Розрахунок загальної кількості вуглеводів для всіх інгредієнтів
  #calculateTotalCarbs(ingredients: RecipeIngredientDetails[]): number {
    return ingredients.reduce((sum, ingredient) => {
      const weightInGrams = this.#calculateGramWeight(ingredient);
      return sum + (ingredient.carbs * weightInGrams / 100);
    }, 0);
  }

  // Розрахунок загальної кількості цукру для всіх інгредієнтів
  #calculateTotalSugar(ingredients: RecipeIngredientDetails[]): number {
    return ingredients.reduce((sum, ingredient) => {
      const weightInGrams = this.#calculateGramWeight(ingredient);
      return sum + (ingredient.sugar * weightInGrams / 100);
    }, 0);
  }

  // Розрахунок харчової цінності на 100г сирого продукту
  calculateRawNutritionPer100g(ingredients: RecipeIngredientDetails[]): NutritionInfo {
    const totalWeight = this.#calculateTotalWeight(ingredients);

    if (totalWeight === 0) {
      return {
        calories: 0,
        protein: 0,
        fat: 0,
        carbs: 0,
        sugar: 0,
        proteinEnergyPercentage: 0,
        fatEnergyPercentage: 0,
        carbsEnergyPercentage: 0
      };
    }

    const totalCalories = this.#calculateTotalCalories(ingredients);
    const totalProtein = this.#calculateTotalProtein(ingredients);
    const totalFat = this.#calculateTotalFat(ingredients);
    const totalCarbs = this.#calculateTotalCarbs(ingredients);
    const totalSugar = this.#calculateTotalSugar(ingredients);

    const caloriesPer100g = (totalCalories / totalWeight) * 100;
    const proteinPer100g = (totalProtein / totalWeight) * 100;
    const fatPer100g = (totalFat / totalWeight) * 100;
    const carbsPer100g = (totalCarbs / totalWeight) * 100;
    const sugarPer100g = (totalSugar / totalWeight) * 100;

    // Розрахунок відсотків для макроелементів для сирого продукту
    const macroPercentages = this.#calculateMacroPercentages(
      proteinPer100g,
      fatPer100g,
      carbsPer100g
    );

    return {
      calories: caloriesPer100g,
      protein: proteinPer100g,
      fat: fatPer100g,
      carbs: carbsPer100g,
      sugar: sugarPer100g,
      proteinEnergyPercentage: macroPercentages.proteinEnergyPercentage,
      fatEnergyPercentage: macroPercentages.fatEnergyPercentage,
      carbsEnergyPercentage: macroPercentages.carbsEnergyPercentage
    };
  }

  // Розрахунок харчової цінності для сирого і готового продукту
  calculateFullNutrition(
    ingredients: RecipeIngredientDetails[],
    cookingMethod: CookingMethod
  ): StructuredNutritionInfo {
    const totalWeight = this.#calculateTotalWeight(ingredients);
    const rawNutrition = this.calculateRawNutritionPer100g(ingredients);

    // Розрахунок нової ваги після приготування з урахуванням втрати води
    const cookedWeight = totalWeight * (1 - cookingMethod.waterLossPercent / 100);

    const cookedProteinPer100g = rawNutrition.protein * cookingMethod.proteinModifier;
    const cookedFatPer100g = rawNutrition.fat * cookingMethod.fatModifier;
    const cookedCarbsPer100g = rawNutrition.carbs * cookingMethod.carbsModifier;
    const cookedCaloriesPer100g = rawNutrition.calories * cookingMethod.caloriesModifier;
    const cookedSugarPer100g = rawNutrition.sugar * cookingMethod.caloriesModifier;

    // Розрахунок відсотків для макроелементів приготовленого продукту
    const cookedMacroPercentages = this.#calculateMacroPercentages(
      cookedProteinPer100g,
      cookedFatPer100g,
      cookedCarbsPer100g
    );

    return {
      totalWeight: totalWeight,
      cookedWeight: cookedWeight,
      raw: {
        calories: rawNutrition.calories,
        protein: rawNutrition.protein,
        fat: rawNutrition.fat,
        carbs: rawNutrition.carbs,
        sugar: rawNutrition.sugar,
        proteinEnergyPercentage: rawNutrition.proteinEnergyPercentage,
        fatEnergyPercentage: rawNutrition.fatEnergyPercentage,
        carbsEnergyPercentage: rawNutrition.carbsEnergyPercentage
      },
      cooked: {
        calories: cookedCaloriesPer100g,
        protein: cookedProteinPer100g,
        fat: cookedFatPer100g,
        carbs: cookedCarbsPer100g,
        sugar: cookedSugarPer100g,
        proteinEnergyPercentage: cookedMacroPercentages.proteinEnergyPercentage,
        fatEnergyPercentage: cookedMacroPercentages.fatEnergyPercentage,
        carbsEnergyPercentage: cookedMacroPercentages.carbsEnergyPercentage
      }
    };
  }
}
