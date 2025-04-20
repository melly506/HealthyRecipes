export interface NutritionPercentage {
  proteinEnergyPercentage: number;
  fatEnergyPercentage: number;
  carbsEnergyPercentage: number;
}

export interface NutritionInfo extends NutritionPercentage {
  calories: number;
  protein: number;
  fat: number;
  carbs: number;
  sugar: number;
}

export interface StructuredNutritionInfo {
  totalWeight: number;
  cookedWeight: number;
  raw: NutritionInfo;
  cooked: NutritionInfo;
}
