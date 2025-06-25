import { BehaviorSubject, Observable } from "rxjs";
import { LoginModel } from "../models/loginmodel";
import { inject, Injectable, signal, WritableSignal } from "@angular/core";
import { HttpBackend, HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable()
export class AuthService {
    public users: LoginModel[] = [
        { username: "sai", password: "password" },
        { username: "kavin", password: "password" }
    ]
    private http :HttpClient = inject(HttpClient)
    public usernameSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
    public username$: Observable<string | null> = this.usernameSubject.asObservable();

    validateUserLogin(user: LoginModel) {
        if (user.username.length < 3) {
            this.usernameSubject.next(null);

        }

        else {
            this.callLoginAPI(user).subscribe(
                {
                    next: (data: any) => {
                        this.usernameSubject.next(user.username);
                        localStorage.setItem("token", data.accessToken)
                    }
                }
            )

        }

    }

    callGetProfile() {
        var token = localStorage.getItem("token")
        const httpHeader = new HttpHeaders({
            'Authorization': `Bearer ${token}`
        })
        return this.http.get('https://dummyjson.com/auth/me', { headers: httpHeader });

    }

    callLoginAPI(user: LoginModel) {
        return this.http.post("https://dummyjson.com/auth/login", user);
    }

    logout() {
        this.usernameSubject.next(null);
    }
}