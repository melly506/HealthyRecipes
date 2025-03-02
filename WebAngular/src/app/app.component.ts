import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import Keycloak from 'keycloak-js';
import { IngredientsService } from  './core/services/ingredients.service';

import { HeaderComponent } from './header/header.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  //таким чином ми підключаємо сервіс, щоб підключити його треба за import
  #ingredients = inject(IngredientsService);
  userInfo: any = {};
  items: any[] = [];
  constructor(private readonly keycloak: Keycloak) {}

  login() {
    this.keycloak.login();
  }

  logout() {
    this.keycloak.logout();
  }

  async test() {
    this.userInfo = await this.keycloak.loadUserProfile();
    this.#ingredients.getIngredients().subscribe(response => {
      this.items = response;
    });
  }
}
