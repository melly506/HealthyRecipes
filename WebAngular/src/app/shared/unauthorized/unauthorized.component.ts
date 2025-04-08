import { Component, inject, Input } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import Keycloak from 'keycloak-js';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  templateUrl: './unauthorized.component.html',
  imports: [
    MatButton,
    MatIcon
  ],
  styleUrl: './unauthorized.component.scss'
})
export class UnauthorizedComponent {
  #keycloakService = inject(Keycloak);

  @Input() message = '';

  async login() {
    await this.#keycloakService.login();
  }
}
