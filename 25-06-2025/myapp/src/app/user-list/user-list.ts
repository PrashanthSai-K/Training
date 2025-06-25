import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { UserModel } from '../models/usermodel';
import { selectAllUsers, selectUserError, selectUserLoading } from '../store/user/users.selector';
import { AddUser } from "../add-user/add-user";
import { AsyncPipe, NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-user-list',
  imports: [AddUser, AsyncPipe, NgIf, NgFor],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {

  users$: Observable<UserModel[]>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;

  constructor(private store: Store) {
    this.users$ = this.store.select(selectAllUsers);
    console.log(this.users$.forEach(next=> console.log(next)));
    
    this.loading$ = this.store.select(selectUserLoading);
    this.error$ = this.store.select(selectUserError);
  }

  ngOnInit(): void {
    this.store.dispatch({ type: '[Users] Load Users' })
  }
}
