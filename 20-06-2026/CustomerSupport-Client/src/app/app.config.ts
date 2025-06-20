import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { BotMessageSquare, CircleUser, CircleUserRound, LucideAngularModule } from 'lucide-angular';
import { ChatService } from './core/services/chat-service';
import { provideHttpClient } from '@angular/common/http';
import { ChatMessageService } from './core/services/chat-message-service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    importProvidersFrom(LucideAngularModule.pick({BotMessageSquare, CircleUser})),
    provideHttpClient(),
    ChatService,
    ChatMessageService
  ],
};
