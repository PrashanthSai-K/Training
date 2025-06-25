import { Component } from '@angular/core';
import { UserModel } from '../models/usermodel';
import { Store } from '@ngrx/store';
import { addUsers } from '../store/user/users.action';

@Component({
  selector: 'app-add-user',
  imports: [],
  templateUrl: './add-user.html',
  styleUrl: './add-user.css'
})
export class AddUser {
  constructor(private store:Store){

  }
  addUser(){
    const nuser = new UserModel(1, "sai", "sai@gmail.com", "pras", "here", "male", "imge.com");
    this.store.dispatch(addUsers({user: nuser}));
  }
}
