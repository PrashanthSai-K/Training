import { BehaviorSubject, Observable } from "rxjs";
import { LoginModel } from "../models/loginmodel";
import { signal, WritableSignal } from "@angular/core";

export class AuthService {
    public users: LoginModel[] = [
        { username: "sai", password: "password" },
        { username: "kavin", password: "password" }
    ]

    public usernameSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
    public username$: Observable<string | null> = this.usernameSubject.asObservable();

    AuthenticateUser(login: LoginModel) {
        let match = this.users.find((user) => user.username == login.username && user.password == login.password);

        if (match) {
            this.usernameSubject.next(match.username);
            localStorage.setItem("username", JSON.stringify(match.username));
            sessionStorage.setItem("username", JSON.stringify(match.username));
            return true;
        } else {
            this.usernameSubject.next(null);
            return false;
        }
    }
}