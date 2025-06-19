import { Routes } from '@angular/router';
import { CreateUser } from './create-user/create-user';
import { UserList } from './user-list/user-list';

export const routes: Routes = [
    {path:"", component: CreateUser},
    {path: "users", component: UserList}
];
