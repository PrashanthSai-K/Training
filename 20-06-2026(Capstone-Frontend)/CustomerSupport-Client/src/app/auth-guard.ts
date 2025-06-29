import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './core/services/auth-service';
import { map, switchMap, take } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  console.log("Called inter");
  
  return authService.getUser().pipe(
    switchMap(() => authService.currentUser$),
    take(1),
    map(user => {
      const reqRoles: string[] = route.data['role'] || [];
      console.log(user && reqRoles.includes(user.role));

      if (user && reqRoles.includes(user.role)) {
        return true;
      }

      router.navigateByUrl('/login');
      return false;
    })
  );
};
