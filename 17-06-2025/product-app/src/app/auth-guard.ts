import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const isLoggedin = localStorage.getItem("token") != null ? true : false;
  if (!isLoggedin) {
    router.navigateByUrl("login");
    return false;
  }
  return true;
};
