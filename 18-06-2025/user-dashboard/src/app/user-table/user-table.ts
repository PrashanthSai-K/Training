import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { UserModel } from '../models/user-model';
import { TitleCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

@Component({
  selector: 'app-user-table',
  imports: [TitleCasePipe, FormsModule],
  templateUrl: './user-table.html',
  styleUrl: './user-table.css'
})

export class UserTable implements OnInit, OnChanges {
  @Input() users: UserModel[] | null = null;
  searchQuery: string = "";
  searchSubject = new Subject();
  filterGender: string = "female";
  filterRole: string = "admin";
  filteredUsers: UserModel[] | null = [];

  searchUsers(query: string) {
    query = query.toLowerCase();
    const result = this.users?.filter((user) => user.firstName.toLowerCase().includes(query) ||
      user.lastName.toLowerCase().includes(query) || user.email.toLowerCase().includes(query));
    if (result != undefined)
      this.filteredUsers = result;
  }

  onSearchInput() {
    this.searchSubject.next(this.searchQuery);
  }

  onRoleInput() {
    console.log(this.filterRole);
    this.filteredUsers = this.users?.filter(user => user.role.toLowerCase() == this.filterRole.toLowerCase()) || null;
  }

  onGenderInput() {
    console.log(this.filterGender);
    this.filteredUsers = this.users?.filter(user => user.gender.toLowerCase() == this.filterGender.toLowerCase()) || null;

  }

  ngOnInit(): void {
    this.filteredUsers = this.users ?? [];
    this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe((query) => this.searchUsers(query as string));
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['users'] && this.users) {
      this.filteredUsers = [...this.users];
    }
  }
}
