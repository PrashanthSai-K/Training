import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
// import { Category } from '../models/category.model';
import { Observable } from 'rxjs';

export interface Category {
    categoryId: number,
    name: string
}

@Injectable({
    providedIn: 'root'
})

export class CategoryService {
    private apiUrl = 'http://localhost:5038/api/category';

    constructor(private http: HttpClient) { }

    getAll(page: number): Observable<any> {
        return this.http.get<any>(`${this.apiUrl}?page=${page}`);
    }

    getById(id: number): Observable<Category> {
        return this.http.get<Category>(`${this.apiUrl}/${id}`);
    }

    create(category: Category): Observable<Category> {
        return this.http.post<Category>(this.apiUrl, category);
    }

    update(category: Category): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/${category.categoryId}`, category);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
