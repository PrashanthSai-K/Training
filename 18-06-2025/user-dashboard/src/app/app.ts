import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { LucideAngularModule, FileIcon, WebhookIcon } from 'lucide-angular';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LucideAngularModule, RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'user-dashboard';
  WebhookIcon = WebhookIcon;
}
