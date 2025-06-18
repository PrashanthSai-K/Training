import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { UserModel } from "../models/user-model";
import { from } from "rxjs";

@Injectable()
export class UserService {
    httpClient: HttpClient = inject(HttpClient);

    createUser(user: UserModel) {
        return from(
            fetch("https://dummyjson.com/users/add", {
                method: "POST",
                headers: { 'content-type': 'application/json' },
                body: JSON.stringify(user)
            }).then((res) => {
                if (!res.ok) throw new Error();
                else return res.json();
            })
        )
    }

    getAllUsers() {
        return this.httpClient.get("https://dummyjson.com/users?limit=200");
    }

    searchUser(query:string) {
        return this.httpClient.get(`https://dummyjson.com/users?q=${query}`);
    }
}