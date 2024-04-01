import { HttpInterceptorFn } from '@angular/common/http';

export const cookieCredentialsInterceptor: HttpInterceptorFn = (req, next) => {
  console.log('cookieCredentialsInterceptor');
  const modifiedReq = req.clone({ withCredentials: true });
  return next(modifiedReq);
};
