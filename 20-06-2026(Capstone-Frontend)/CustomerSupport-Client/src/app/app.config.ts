import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { BotMessageSquare, CircleUser, LucideAngularModule, MessageCirclePlus, SendHorizontal, Upload } from 'lucide-angular';
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
    importProvidersFrom(LucideAngularModule.pick({ BotMessageSquare, CircleUser, MessageCirclePlus, Upload, SendHorizontal })),
    provideHttpClient(withInterceptors([authInterceptor])),
    // {
    //   provide: "APP_INIT",
    //   useFactory: (authService: AuthService) => {
    //     return authService.getUser();
    //   },
    //   deps: [AuthService],
    //   multi: true
    // },
    ChatService,
    ChatHubService,
    AuthService,
    CustomerService
  ],
};
