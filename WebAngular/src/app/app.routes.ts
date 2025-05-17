import { Routes } from '@angular/router';

import { ProfileComponent } from './profile/profile.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { ManageRecipeComponent } from './manage-recipe/manage-recipe.component';
import { QuickRecipesComponent } from './quick-recipes/quick-recipes.component';
import { ArticlesComponent } from './articles/articles.component';
import { HomeComponent } from './home/home.component';
import { MyRecipesComponent } from './my-recipes/my-recipes.component';

export const routes: Routes = [
  { path: '', component: HomeComponent }, // Головна сторінка
  { path: 'profile', component: ProfileComponent },// Мій профіль
  { path: 'profile/:id', component: ProfileComponent },// Публічний профіль
  { path: 'favorites', component: FavoritesComponent },
  { path: 'my-recipes', component: MyRecipesComponent },
  { path: 'recipe', component: ManageRecipeComponent }, // Створення рецепта
  { path: 'recipe/:id', component: ManageRecipeComponent }, // Редагування рецепта
  { path: 'quick-recipes', component: QuickRecipesComponent }, // Швидкі рецепти
  { path: 'articles', component: ArticlesComponent }, // Статті
];
