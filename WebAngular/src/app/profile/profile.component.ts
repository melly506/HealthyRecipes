import {Component, effect, inject, signal} from '@angular/core';

import Keycloak from 'keycloak-js';
import { IngredientsService } from '../core/services/ingredients.service';

import {KEYCLOAK_EVENT_SIGNAL, KeycloakEventType, ReadyArgs, typeEventArgs} from 'keycloak-angular';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  //таким чином ми підключаємо сервіс, щоб підключити його треба за import
  #ingredients = inject(IngredientsService);
  userInfo = signal<any>({});
  items: any[] = [];
  item: any;
  authenticated: boolean = false;

  constructor(private readonly keycloak: Keycloak) {
    const keycloakSignal = inject(KEYCLOAK_EVENT_SIGNAL);


    effect(async () => {
      const keycloakEvent = keycloakSignal();

      if (keycloakEvent.type === KeycloakEventType.Ready) {
        this.authenticated = typeEventArgs<ReadyArgs>(keycloakEvent.args);
        if (this.authenticated) {
          const userProfile = await this.keycloak.loadUserProfile();
          this.userInfo.set(userProfile);
        }
      }

      if (keycloakEvent.type === KeycloakEventType.AuthLogout) {
        this.authenticated = false;
        this.userInfo.set({ });
      }
    });
  }


  login() {
    this.keycloak.login();
  }

  logout() {
    this.keycloak.logout();
  }

  async test() {
    this.#ingredients.getIngredients().subscribe(response => {
      this.items = response;
    });
  }
}
