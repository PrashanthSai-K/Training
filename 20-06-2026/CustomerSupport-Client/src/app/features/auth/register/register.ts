import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-register',
  imports: [LucideAngularModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  constructor(private router:Router){
  }

  redirectLogin(){
    this.router.navigateByUrl("/login");
  }
}
