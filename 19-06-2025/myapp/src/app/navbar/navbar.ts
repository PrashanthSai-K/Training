import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../service/authservice';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  username: string = "";
  private authservice:AuthService = inject(AuthService);

  constructor(){
    this.authservice.username$.subscribe({
      next:(data) => {
        this.username = data as string;
      },
    })
  }
}
