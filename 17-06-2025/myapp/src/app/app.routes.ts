import { Routes } from '@angular/router';
import { Products } from './products/products';
import { SearchProducts } from './search-products/search-products';
import { Login } from './login/login';
import { Home } from './home/home';
import { Profile } from './profile/profile';
import { authGuard } from './auth-guard';

export const routes: Routes = [
    { path: 'login', component: Login },
    {
        path: 'home/:un', component: Home, children: [
            { path: 'products', component: Products },
            { path: 'search', component: SearchProducts },
        ]
    },
    {path: "profile", component: Profile, canActivate: [authGuard]}
];
