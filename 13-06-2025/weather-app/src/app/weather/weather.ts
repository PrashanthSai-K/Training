import { TitleCasePipe } from '@angular/common';
import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WeatherService } from '../service/weather-service';

@Component({
  selector: 'app-weather',
  imports: [FormsModule],
  templateUrl: './weather.html',
  styleUrl: './weather.css'
})
export class Weather implements OnInit {

  public city: WritableSignal<string> = signal('');
  private weatherService: WeatherService = inject(WeatherService);
  public weather: WritableSignal<any> = signal<any>(null);

  ngOnInit() {
    console.log("called");
    this.weatherService.weather$.subscribe({
      next: (data) => {
        this.weather.set(data);
      },
      error: (err) => {
        this.weather.set(null)
        console.log(err);
      }
    })
  }

  onSearch() {
    this.weatherService.GetWeatherData(this.city());
  }
}
