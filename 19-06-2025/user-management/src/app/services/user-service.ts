import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { UserModel } from "../models/user-model";
import { BehaviorSubject, combineLatest, debounceTime, distinctUntilChanged, from, map, startWith } from "rxjs";

@Injectable()
export class UserService {
    httpClient: HttpClient = inject(HttpClient);
    userlist: UserModel[] = [
        new UserModel("Sai", "Prashanth", "sai@gmail.com", "sai123", "", "user"),
        new UserModel("Hari", "Haran", "hari@gmail.com", "hari123", "", "admin"),
        new UserModel("Kavin", "Raj", "kavin@gmail.com", "kavin123", "", "user"),
        new UserModel("Anita", "Joseph", "anita.j@gmail.com", "anita123", "", "admin"),
        new UserModel("Vikram", "Singh", "vikram@gmail.com", "vikram123", "", "user"),
        new UserModel("Divya", "Sharma", "divya@gmail.com", "divya123", "", "admin"),
        new UserModel("Arjun", "Mehta", "arjun@gmail.com", "arjun123", "", "user"),
        new UserModel("Neha", "Patel", "neha@gmail.com", "neha123", "", "admin"),
        new UserModel("Rahul", "Kumar", "rahul@gmail.com", "rahul123", "", "user"),
        new UserModel("Sneha", "Reddy", "sneha@gmail.com", "sneha123", "", "user"),

    ]

    users = new BehaviorSubject<UserModel[]>(this.userlist);
    users$ = this.users.asObservable();

    searchSubject$ = new BehaviorSubject<string>("");
    filterRoleSubject$ = new BehaviorSubject<string>("all");

    createUser(user: UserModel) {
        this.users.value.push(user);
        this.users.next(this.users.value);
    }

    filteredUser$ = combineLatest([this.users, this.searchSubject$.pipe(startWith("")), this.filterRoleSubject$.pipe(startWith("all"))]).pipe(
        debounceTime(500),
        distinctUntilChanged(),
        map(([users, query, role]) => {
            return users.filter(user => {
                const matchQuery = user.firstName.toLowerCase().includes(query) || user.lastName.toLowerCase().includes(query) || user.email.toLowerCase().includes(query);
                const matchRole = role == 'all' || user.role == role;

                return matchQuery && matchRole;
            })
        })
    )
}
