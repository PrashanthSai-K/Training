import { Routes } from '@angular/router';
import { Products } from './products/products';
import { SearchProducts } from './search-products/search-products';
import { Login } from './login/login';

export const routes: Routes = [
    { path: 'products', component: Products },
    { path: 'search', component: SearchProducts },
    { path: 'login', component: Login }

];
