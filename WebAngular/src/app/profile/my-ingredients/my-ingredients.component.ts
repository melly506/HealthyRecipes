import { ChangeDetectorRef, Component, DestroyRef, inject, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { debounceTime, filter } from 'rxjs';

import { IngredientsService } from '../../core/services';
import { Ingredient, IngredientDetailed } from '../../core/interfaces';
import {
  ManageIngredientComponent
} from '../../shared/manage-ingredients/manage-ingredient/manage-ingredient.component';
import { UnitPipe } from '../../shared/pipes/unit.pipe';

@Component({
  selector: 'app-my-ingredients',
  standalone: true,
  imports: [
    MatTableModule,
    MatSortModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    UnitPipe
  ],
  templateUrl: './my-ingredients.component.html',
  styleUrl: './my-ingredients.component.scss'
})
export class MyIngredientsComponent {
  @ViewChild(MatSort) sort!: MatSort;
  #ingredientsService = inject(IngredientsService);
  #dialog = inject(MatDialog);
  #dr = inject(DestroyRef);
  #cd = inject(ChangeDetectorRef);

  myIngredients: IngredientDetailed[] = [];
  displayedColumns: string[] = ['name', 'calories', 'fat', 'protein', 'carbs', 'sugar', 'unit', 'actions'];

  ngOnInit(): void {
    this.loadMyIngredients();
  }

  ngAfterViewInit() {
    this.sort.sortChange
      .pipe(
        debounceTime(10),
        takeUntilDestroyed(this.#dr)
      )
      .subscribe(sort => {
        this.#sortIngredients(sort);
      });
  }


  loadMyIngredients(): void {
    this.#ingredientsService.getMyIngredients()
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe(ingredients => {
        this.myIngredients = ingredients;
      });
  }

  #sortIngredients(sort: Sort = { active: 'name', direction: 'desc' }): void {
    this.myIngredients  = [...this.myIngredients ].sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      const key = sort.active as keyof Ingredient;

      // Use the active key to directly access the property
      return this.#compare(a[key], b[key], isAsc);
    });
    this.#cd.detectChanges();
  }

  #compare(a: any, b: any, isAsc: boolean): number {
    // Handle string comparison
    if (typeof a === 'string' && typeof b === 'string') {
      return (a.localeCompare(b)) * (isAsc ? 1 : -1);
    }

    // Handle number comparison
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }

  openEditIngredient(ingredient: IngredientDetailed): void {
    const dialogRef = this.#dialog.open(ManageIngredientComponent, {
      width: '1200px',
      panelClass: 'app-big-modal',
      data: { ingredientId: ingredient.id }
    });

    dialogRef.afterClosed()
      .pipe(
        takeUntilDestroyed(this.#dr),
        filter((updatedIngredient): updatedIngredient is IngredientDetailed => !!updatedIngredient)
      )
      .subscribe(updatedIngredient => {
        this.myIngredients = this.myIngredients.map(item => {
          if (item.id === updatedIngredient.id) {
            return {
              ...item,
              ...updatedIngredient
            };
          }
          return item;
        });
      });
  }

}
