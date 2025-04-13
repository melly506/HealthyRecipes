export interface Recipe {
  name: string;
  imageUrl: string;
  cookingTime: number;
  description: string;
  instructions: string;
}

export interface RecipeForUpdate extends Recipe {
  recipeIngridientsAssign: {
    count: number;
    ingridientId: string
  }[];
  foodTypeIds: string[];
  dietIds: string[];
  seasonIds: string[];
  dishTypeIds: string[];
}
