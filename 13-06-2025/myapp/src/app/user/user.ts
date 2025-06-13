import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { AuthService } from '../service/authservice';

@Component({
  selector: 'app-user',
  imports: [],
  templateUrl: './user.html',
  styleUrl: './user.css'
})
export class User {
  public user: string | null = null;

  constructor(private authService: AuthService) {

    this.authService.username$.subscribe({
      next: (data) => {
        this.user = data;
      },
      error: (err) => {
        alert(err);
      }
    })
  }
}
