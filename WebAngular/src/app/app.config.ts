import { provideHttpClient } from "@angular/common/http";
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import {
    provideKeycloak,
    createInterceptorCondition,
    IncludeBearerTokenCondition,
    INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG
} from 'keycloak-angular';

import { routes } from './app.routes';

const urlCondition = createInterceptorCondition<IncludeBearerTokenCondition>({
    urlPattern: /^(http:\/\/localhost:4378)(\/.*)?$/i,
    bearerPrefix: 'Bearer'
});

export const appConfig: ApplicationConfig = {
    providers: [
        provideKeycloak({
            config: {
                url: 'http://localhost:3255/',
                realm: 'DevRealm',
                clientId: 'recipe_management.web'
            },
            initOptions: {
                onLoad: 'check-sso',
                silentCheckSsoRedirectUri: window.location.origin + '/silent-check-sso.html'
            }
        }),
        {
            provide: INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
            useValue: [urlCondition]
        },
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(routes),
        provideHttpClient()
    ]
};
