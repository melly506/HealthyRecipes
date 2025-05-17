import { SystemFields } from './system.interface';

export interface Ingredient {
  id: string;
  name: string;
  calories: number;
  unit: string;
  fat: number;
  carbs: number;
  protein: number;
  sugar: number;
}

export interface IngredientDetailed extends Ingredient, SystemFields {

}

export interface IngredientForCreation {
  name: string;
  calories: number;
  unit: string;
  fat: number;
  carbs: number;
  protein: number;
  sugar: number;
}

export interface ManageIngredientDialogData {
  ingredientId?: string | null;
}
