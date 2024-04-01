import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { UserService } from 'src/app/services/user.service';

export const authorizeInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
) => {
  console.log('AuthorizeInterceptorFn');
  const userService = inject(UserService);
  return next(req).pipe(
    catchError((error) => {
      if (
        error instanceof HttpErrorResponse &&
        error.url?.startsWith(loginUrl)
      ) {
        userService.setIsAuthenticated(false);
        window.location.href = `https://localhost:4200/login`;
      }
      return throwError(() => error);
    }),
    // HACK: As of .NET 8 preview 5, some non-error responses still need to be redirected to the login page.
    map((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse && event.url?.startsWith(loginUrl)) {
        userService.setIsAuthenticated(false);
        window.location.href = `https://localhost:4200/login`;
      }
      return event;
    })
  );
};

const loginUrl: string = 'https://localhost:5001/Account/Login';
