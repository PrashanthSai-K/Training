import { Routes } from '@angular/router';
import { Home } from './home/home';
import { About } from './about/about';
import { ProductById } from './product-by-id/product-by-id';
import { Landing } from './landing/landing';
import { Login } from './login/login';
import { authGuard } from './auth-guard';

export const routes: Routes = [
    { path: "", component: Landing },
    { path: 'products', component: Home, canActivate: [authGuard] },
    { path: 'products/:id', component: ProductById, canActivate: [authGuard] },
    { path: 'about', component: About },
    { path: 'login', component: Login }
];
