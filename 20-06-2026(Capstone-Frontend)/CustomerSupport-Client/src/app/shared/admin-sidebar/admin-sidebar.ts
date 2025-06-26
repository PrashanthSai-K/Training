import { Component } from '@angular/core';
import { AuthService } from '../../core/services/auth-service';

@Component({
  selector: 'app-admin-sidebar',
  imports: [],
  templateUrl: './admin-sidebar.html',
  styleUrl: './admin-sidebar.css'
})
export class AdminSidebar {

  constructor(public authservice:AuthService){
    
  }
}
