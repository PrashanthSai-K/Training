import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { ChatTemplate } from './features/chat/chat-template/chat-template';

export const routes: Routes = [
    { path: "login", component: Login },
    { path: "register", component: Register },
    { path: "chat", component: ChatTemplate }
];
