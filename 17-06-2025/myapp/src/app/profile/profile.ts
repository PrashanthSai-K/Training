import { Component, inject } from '@angular/core';
import { UserModel } from '../models/usermodel';
import { AuthService } from '../service/authservice';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile {
     authService = inject(AuthService);
     profileData:UserModel = new UserModel();

     constructor(){
        this.authService.callGetProfile().subscribe({
          next:(data:any)=>{
            this.profileData = UserModel.fromForm(data);
          }
        })
     }

}
