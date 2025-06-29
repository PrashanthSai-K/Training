import { inject, Injectable } from "@angular/core";
import { LoginModel, LoginResponse } from "../models/login";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, catchError, of, tap, throwError } from "rxjs";
import { CustomerModel } from "../models/register";
import { Router } from "@angular/router";
import { User } from "../models/user";

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private authUrl = "http://localhost:5124/api/v1/auth";
    private httpClient = inject(HttpClient);
    private router = inject(Router);

    routeSubject = new BehaviorSubject<string>("");
    route$ = this.routeSubject.asObservable();


    private currentUserSubject = new BehaviorSubject<User | null>(null);
    currentUser$ = this.currentUserSubject.asObservable();

    constructor() {
        this.initializeUser();
    }

    initializeUser() {
        const token = this.getAccessToken();
        if (!token)
            this.currentUserSubject.next(null);
        else
            this.getUser().subscribe();
    }

    getAccessToken() {
        return localStorage.getItem("token");
    }

    getRefreshToken() {
        return localStorage.getItem("refreshToken");
    }

    loginUser(login: LoginModel) {
        return this.httpClient.post<LoginResponse>(`${this.authUrl}/login`, login).pipe(
            tap((response: LoginResponse) => {
                localStorage.setItem("token", response.accessToken);
                localStorage.setItem("refreshToken", response.refreshToken);
                localStorage.setItem("username", response.username);
                this.getUser().subscribe();
            })
        );
    }

    refreshToken() {
        const username = localStorage.getItem('username');
        const refreshToken = this.getRefreshToken();
        if (!refreshToken || !username) return throwError(() => new Error("Refresh Token / Username missing"));
        const headers = new HttpHeaders().set('skip-refresh-interceptor', 'true');

        return this.httpClient.post<LoginResponse>(`${this.authUrl}/refresh`, { username: username, refreshToken: refreshToken }, { headers: headers }).pipe(
            tap((response: LoginResponse) => {
                localStorage.setItem("token", response.accessToken);
                localStorage.setItem("refreshToken", response.refreshToken);
                localStorage.setItem("username", response.username);
            })
        )
    }

    getUser() {
        return this.httpClient.get<User>(`${this.authUrl}/me`, {
            headers:
                new HttpHeaders().set('skip-refresh-interceptor', 'true').set('Authorization', `Bearer ${this.getAccessToken()}`)
        }).pipe(
            tap((response) => {
                this.currentUserSubject.next(response);
            }),
            catchError(err => {
                console.error("Me failed:", err);
                this.currentUserSubject.next(null);
                if (err?.status == 401)
                    this.router.navigateByUrl("/login");
                return of(null);
            })
        )
    }

    forgotPassword(email: string) {
        return this.httpClient.post(`${this.authUrl}/forgotPassword`, { email: email });
    }

    resetPassword(token: string, email: string, password: string) {
        return this.httpClient.post(`${this.authUrl}/resetPassword`, { token: token, email: email, password: password });
    }

    logoutUser() {
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("username");
        this.currentUserSubject.next(null);
        this.router.navigateByUrl("/login");
    }
}