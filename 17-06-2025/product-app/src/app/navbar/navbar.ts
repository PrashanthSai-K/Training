import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../service/auth-service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements OnInit {
  router: Router = inject(Router);
  isLoggedin: WritableSignal<boolean> = signal<boolean>(false);
  authService: AuthService = inject(AuthService);

  onLogout() {
    this.authService.logoutUser();
    this.isLoggedin.set(false);
    this.router.navigateByUrl("login");
  }

  ngOnInit(): void {
    let token = localStorage.getItem('token') || null;
    if (token != null)
      this.isLoggedin.set(true);

    this.authService.loggedIn$.subscribe({
      next: (data) => {
        if (data == "logged in")
          this.isLoggedin.set(true);
      }
    })
  }
}
