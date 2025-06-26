import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { BotMessageSquare, CircleArrowLeft, CircleUser, CircleX, LucideAngularModule, MessageCirclePlus, MessageSquarePlus, SendHorizontal, TicketCheck, TicketPercent, Upload } from 'lucide-angular';
import { ChatService } from './core/services/chat-service';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './auth-interceptor';
import { ChatHubService } from './core/services/chat-hub-service';
import { AuthService } from './core/services/auth-service';
import { CustomerService } from './core/services/customer-service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    importProvidersFrom(LucideAngularModule.pick({ BotMessageSquare, CircleUser, MessageCirclePlus, 
    Upload, SendHorizontal, CircleArrowLeft, CircleX, MessageSquarePlus, TicketPercent, TicketCheck })),
    provideHttpClient(withInterceptors([authInterceptor])),
    ChatService,
    ChatHubService,
    AuthService,
    CustomerService
  ],
};
