import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { ChatTemplate } from './features/chat/chat-template/chat-template';
import { Landing } from './features/landing/landing';
import { Dashboard } from './features/dashboard/dashboard';
import { AgentManagement } from './features/agent-management/agent-management';
import { CustomerManagement } from './features/customer-management/customer-management';
import { ForgotPassword } from './features/auth/forgot-password/forgot-password';
import { ResetPassword } from './features/auth/reset-password/reset-password';
import { authGuard } from './auth-guard';

export const routes: Routes = [
    
    { path: "", component: Landing },
    { path: "login", component: Login },
    { path: "forgotPassword", component: ForgotPassword },
    { path: "resetPassword", component: ResetPassword },
    { path: "register", component: Register },
    {
        path: "chat", component: ChatTemplate,
        canActivate: [authGuard],
        data: {
            role: ['Agent', 'Customer']
        }
    },
    {
        path: "dashboard", component: Dashboard,
        canActivate: [authGuard],
        data: {
            role: ['Admin', 'Agent', 'Customer']
        }
    },
    {
        path: "agent", component: AgentManagement,
        canActivate: [authGuard],
        data: {
            role: ['Admin']
        }
    },
    {
        path: "customer", component: CustomerManagement,
        canActivate: [authGuard],
        data: {
            role: ['Admin']
        }
    }
];
