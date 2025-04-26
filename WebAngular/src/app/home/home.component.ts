import { Component, inject, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { RecipesListComponent } from '../shared/recipes-list/recipes-list.component';

@Component({
    selector: 'app-home',
    standalone: true,
    imports: [
        RecipesListComponent
    ],
    templateUrl: './home.component.html',
    styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
    #title = inject(Title);

    ngOnInit(): void {
        this.#title.setTitle('Green Spoon');
    }
}
