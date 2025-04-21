import { SystemFields } from './system.interface';
import { FoodType } from './food-type.interface';
import { Diet } from './diet.interface';
import { Season } from './season.interface';
import { DishType } from './dish-type.interface';
import { RecipeIngredientDetails } from './recipe-ingredient';

export interface Recipe {
  name: string;
  imageUrl: string;
  cookingTime: number;
  description: string;
  instructions: string;
}

export interface RecipeDetailed extends Recipe, SystemFields {
  foodType: FoodType[];
  diet: Diet[];
  season: Season[];
  dishType: DishType[];
  isLiked: boolean;
}

export interface RecipeResponse {
  ingridientsDetails: RecipeIngredientDetails[];
  recipe: RecipeDetailed;
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
