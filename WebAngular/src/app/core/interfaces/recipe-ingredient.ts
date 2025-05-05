export interface RecipeIngredientDetails {
  id?: string;
  recipeId?: string;
  ingredientId: string;
  count: number | null;
  ingredientName: string;
  calories: number;
  unit: string;
  fat: number;
  carbs: number;
  protein: number;
  sugar: number;
}
