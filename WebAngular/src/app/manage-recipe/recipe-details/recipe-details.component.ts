import { Component, Input } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';

import { RecipePictureComponent } from '../../shared/recipe-picture/recipe-picture.component';
import { CookingTimeFormatPipe } from '../../shared/pipes/cooking-time-format.pipe';
import { RecipeResponse, User } from '../../core/interfaces';
import { RecipeTagIconsComponent } from '../../shared/recipe-tag-icons/recipe-tag-icons.component';
import { ManageIngredientsComponent } from '../../shared/manage-ingredients/manage-ingredients.component';

@Component({
  selector: 'app-recipe-details',
  standalone: true,
  templateUrl: './recipe-details.component.html',
  imports: [
    ReactiveFormsModule,
    RecipePictureComponent,
    CookingTimeFormatPipe,
    MatIcon,
    RecipeTagIconsComponent,
    ManageIngredientsComponent
  ],
  styleUrl: './recipe-details.component.scss'
})
export class RecipeDetailsComponent {
  @Input() user?: User | null;
  @Input() recipeResponse?: RecipeResponse;
}
