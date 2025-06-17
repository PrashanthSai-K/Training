import { Subject } from "rxjs";

export class AuthService {
    loggedIn = new Subject();
    loggedIn$ = this.loggedIn.asObservable();

    authenticateUser() {
        localStorage.setItem("token", "xm380djn280db81n280sjn qwudh210nd bu0d1h2nd2");
        this.loggedIn.next("logged in");
    }

    logoutUser() {
        localStorage.removeItem("token");
        this.loggedIn.next("logged out");
    }
}