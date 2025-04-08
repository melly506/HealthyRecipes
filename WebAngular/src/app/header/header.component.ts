import { Component, DestroyRef, effect, inject, OnInit } from '@angular/core';
import { MatIconButton } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIcon } from '@angular/material/icon';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { KEYCLOAK_EVENT_SIGNAL, KeycloakEventType, ReadyArgs, typeEventArgs } from 'keycloak-angular';
import Keycloak from 'keycloak-js';

import { UsersService } from '../core/services';
import { User } from '../core/interfaces';
import { UserPictureComponent } from '../shared/user-picture/user-picture.component';


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, MatIconButton, MatIcon, MatMenuModule, UserPictureComponent],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  #usersService = inject(UsersService);
  #keycloakService = inject(Keycloak);
  #dr = inject(DestroyRef);
  public authenticated = false;
  public user?: User;
  
  constructor() {
    const keycloakSignal = inject(KEYCLOAK_EVENT_SIGNAL);

    effect(async () => {
      const keycloakEvent = keycloakSignal();

      if (keycloakEvent.type === KeycloakEventType.Ready) {
        this.authenticated = typeEventArgs<ReadyArgs>(keycloakEvent.args);
        this.#loadCurrentUser();
      }

      if (keycloakEvent.type === KeycloakEventType.AuthLogout) {
        this.authenticated = false;
      }
    });
  }

  ngOnInit() {
    this.#usersService.userCache
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe(userCache => {
        if (userCache && userCache?.id === this.user?.id) {
          this.user = userCache;
        }
      })
  }

  async logout() {
    await this.#keycloakService.logout();
  }

  async login() {
    await this.#keycloakService.login();
  }

  #loadCurrentUser() {
    this.#usersService.getCurrentUser()
      .pipe(
        takeUntilDestroyed(this.#dr)
      )
      .subscribe(user => {
        this.user = user;
      });
  }
}
