import { Routes } from '@angular/router';

import { ProfileComponent } from './profile/profile.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { ManageRecipeComponent } from './manage-recipe/manage-recipe.component';
import { QuickRecipesComponent } from './quick-recipes/quick-recipes.component'; //  компонент для швидких рецептів
import { ArticlesComponent } from './articles/articles.component'; // Додайте компонент для статей
import { HomeComponent } from './home/home.component'; // Додайте компонент для головної сторінки


export const routes: Routes = [
  { path: '', component: HomeComponent }, // Головна сторінка
  { path: 'profile', component: ProfileComponent },// Реєстрація 
  { path: 'favorites', component: FavoritesComponent },// Вподобання-Збережені рецепти 
  { path: 'manage-recipe', component: ManageRecipeComponent },// Створення рецепту
  { path: 'quick-recipes', component: QuickRecipesComponent }, // Швидкі рецепти
  { path: 'articles', component: ArticlesComponent }, // Статті
];
