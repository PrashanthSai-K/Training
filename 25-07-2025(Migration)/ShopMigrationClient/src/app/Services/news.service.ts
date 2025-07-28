import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface News {
    newsId?: number;
    title: string;
    shortDescription: string;
    image: string;
    content: string;
    createdDate: string;
    status: number;
    userId: number;
    user?: { username: string };
}


export interface PagedNewsResponse {
    items: News[];
    pageNumber: number;
    pageCount: number;
}

@Injectable({
    providedIn: 'root'
})
export class NewsService {
    private baseUrl = 'http://localhost:5038/api/news';

    constructor(private http: HttpClient) { }

    getNews(page: number = 1): Observable<News[]> {
        return this.http.get<News[]>(`${this.baseUrl}?page=${page}`);
    }

    getAll(): Observable<News[]> {
        return this.http.get<News[]>(this.baseUrl);
    }

    getById(id: number): Observable<News> {
        return this.http.get<News>(`${this.baseUrl}/${id}`);
    }

    create(news: News): Observable<News> {
        return this.http.post<News>(this.baseUrl, news);
    }

    update(id: number, news: News): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, news);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}
