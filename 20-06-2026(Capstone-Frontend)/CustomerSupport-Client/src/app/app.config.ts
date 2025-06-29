import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { BotMessageSquare, CircleArrowLeft, CircleUser, CircleX, ClipboardCopy, KeyRound, LayoutDashboard, Lock, LucideAngularModule, MessageCirclePlus, MessageSquarePlus, MoreVertical, SendHorizontal, ShieldUser, TicketCheck, TicketPercent, Upload, UserCog, UserPlus } from 'lucide-angular';
import { ChatService } from './core/services/chat-service';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './auth-interceptor';
import { ChatHubService } from './core/services/chat-hub-service';
import { AuthService } from './core/services/auth-service';
import { CustomerService } from './core/services/customer-service';
import { AgentService } from './core/services/agent-service';
import { DashboardService } from './core/services/dashboard-service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    importProvidersFrom(LucideAngularModule.pick({
      BotMessageSquare, CircleUser, MessageCirclePlus, ShieldUser, UserCog, UserPlus, ClipboardCopy,
      Upload, SendHorizontal, CircleArrowLeft, CircleX, MessageSquarePlus, TicketPercent, TicketCheck, LayoutDashboard, MoreVertical, KeyRound, Lock
    })),
    provideHttpClient(withInterceptors([authInterceptor])),
    ChatService,
    ChatHubService,
    AuthService,
    CustomerService,
    AgentService,
    DashboardService
  ],
};
