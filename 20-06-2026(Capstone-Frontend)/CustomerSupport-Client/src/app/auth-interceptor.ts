import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './core/services/auth-service';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  if (req.headers.get('skip-refresh-interceptor') === 'true') {
    return next(req);
  }

  const authService = inject(AuthService);
  const token = authService.getAccessToken();


  const authReq = token
    ? req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    })
    : req;

  return next(authReq).pipe(
    catchError((err) => {
      console.log("🚨 Interceptor error:", err);
      const isLoginRequest = req.url.includes('/login'); 
      if (isLoginRequest) {
        return throwError(() => err);
      }

      if (err.status === 401) {
        return authService.refreshToken().pipe(
          switchMap(() => {
            const newToken = authService.getAccessToken();

            if (!newToken) {
              authService.logoutUser();
              return throwError(() => err);
            }
            const retryReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`,
                'x-refresh-retry': 'true'
              }
            });
            return next(retryReq);
          }),
          catchError((refreshError) => {
            authService.logoutUser();
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => err);
    })
  );
};
