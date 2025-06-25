import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { ChatTemplate } from './features/chat/chat-template/chat-template';
import { Landing } from './features/landing/landing';

export const routes: Routes = [
    { path: "", component: Landing },
    { path: "login", component: Login },
    { path: "register", component: Register },
    { path: "chat", component: ChatTemplate }
];
