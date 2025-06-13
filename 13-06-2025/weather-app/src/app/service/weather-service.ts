import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, catchError, of } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class WeatherService {
    public apiUrl: string = "http://api.weatherapi.com/v1/current.json";
    public apiKey: string = "eb8e45bdc9a34f54a6d113638251306";
    private httpClient: HttpClient = inject(HttpClient);

    private weatherSubject = new BehaviorSubject<any>(null);
    public weather$ = this.weatherSubject.asObservable();

    GetWeatherData(city: string) {
        let url = `${this.apiUrl}?key=${this.apiKey}&q=${city}&aqi=no`;
        console.log(url);
        this.httpClient.get(url).pipe(
            catchError((err) => {
                this.weatherSubject.next({ error: "City error" })
                console.log(err);
                return of(null);
            })
        ).subscribe((data) => {
            if (data) {
                this.weatherSubject.next(data);
            }
        })
    }
}