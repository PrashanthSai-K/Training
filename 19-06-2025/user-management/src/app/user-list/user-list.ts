import { Component, OnInit } from '@angular/core';
import { UserModel } from '../models/user-model';
import { UserService } from '../services/user-service';
import { User } from '../user/user';
import { combineLatest, debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  imports: [User, FormsModule],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {

  users: UserModel[] | null = null;
  searchQuery: string = "";
  filterRole: string = "all";


  constructor(private userService: UserService) {
  }

  onsearch() {
    console.log(this.searchQuery);
    this.userService.searchSubject$.next(this.searchQuery);
  }
  onFilter() {
    this.userService.filterRoleSubject$.next(this.filterRole);
  }

  ngOnInit(): void {
    this.userService.filteredUser$.subscribe({
      next: (data) => {
        console.log(data);
        this.users = data as UserModel[];
      }
    });
  }
}
