import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
export interface Color {
    colorId: number;
    color1: string;
}

@Injectable({
    providedIn: 'root'
})
export class ColorService {
    private apiUrl = 'http://localhost:5038/api/colors';

    constructor(private http: HttpClient) { }

    getAll(): Observable<Color[]> {
        return this.http.get<Color[]>(this.apiUrl);
    }

    getById(id: number): Observable<Color> {
        return this.http.get<Color>(`${this.apiUrl}/${id}`);
    }

    create(color: Color): Observable<Color> {
        console.log(color);

        return this.http.post<Color>(this.apiUrl, { color1: color.color1 });
    }

    update(id: number, color: Color): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/${id}`, {colorId: id, color1: color.color1 });
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
