import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIcon } from '@angular/material/icon';
import { MatChip, MatChipSet } from '@angular/material/chips';

import { RecipePictureComponent } from '../../shared/recipe-picture/recipe-picture.component';
import { CookingTimeFormatPipe } from '../../shared/pipes/cooking-time-format.pipe';
import { RecipeResponse, User } from '../../core/interfaces';
import { ManageIngredientsComponent } from '../../shared/manage-ingredients/manage-ingredients.component';
import { UserPictureComponent } from '../../shared/user-picture/user-picture.component';
import { CreatedByWrapperComponent } from '../../shared/created-by-wrapper';
import { UserCommentsComponent } from '../user-comments/user-comments.component';

@Component({
  selector: 'app-recipe-details',
  standalone: true,
  templateUrl: './recipe-details.component.html',
  imports: [
    ReactiveFormsModule,
    RecipePictureComponent,
    CookingTimeFormatPipe,
    MatIcon,
    ManageIngredientsComponent,
    MatChipSet,
    MatChip,
    UserPictureComponent,
    CreatedByWrapperComponent,
    RouterLink,
    UserCommentsComponent
  ],
  styleUrl: './recipe-details.component.scss'
})
export class RecipeDetailsComponent {
  @Input() user?: User | null;
  @Input() recipeResponse?: RecipeResponse;
}
