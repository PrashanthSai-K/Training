import { Routes } from '@angular/router';
import { Dashboard } from './dashboard/dashboard';
import { CreateUser } from './create-user/create-user';

export const routes: Routes = [
    {path:"", component: Dashboard},
    {path:"create-user", component: CreateUser}
];
