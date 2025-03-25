import { Component, inject } from '@angular/core';
import { IngredientsService } from '../core/services/ingredients.service';
import Keycloak from 'keycloak-js';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  //таким чином ми підключаємо сервіс, щоб підключити його треба за import
  #ingredients = inject(IngredientsService);
  userInfo: any = {};
  items: any[] = [];
  item: any;
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
    this.#ingredients.getIngredientById('c6434a10-7db4-4600-b987-99d60315d039').subscribe(response =>{
      this.item = response;
    });
  }
}
