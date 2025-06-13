import { TitleCasePipe } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Weather } from "./weather/weather";

@Component({
  selector: 'app-root',
  imports: [Weather],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'weather-app';
}
