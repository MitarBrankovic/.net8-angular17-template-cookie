import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptors,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { cookieCredentialsInterceptor } from 'src/interceptors/cookie-credentials.interceptor';
import { provideToastr } from 'ngx-toastr';
import { authorizeInterceptor } from 'src/interceptors/authorize.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    provideToastr(),
    provideHttpClient(
      // withInterceptorsFromDi(),  // If using non-functional interceptors
      withInterceptors([cookieCredentialsInterceptor, authorizeInterceptor])
    ),
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: AuthorizeInterceptor,
    //   multi: true,
    // },
  ],
};
