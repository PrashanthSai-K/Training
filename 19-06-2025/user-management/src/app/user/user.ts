import { Component, Input } from '@angular/core';
import { UserModel } from '../models/user-model';
import { TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-user',
  imports: [TitleCasePipe],
  templateUrl: './user.html',
  styleUrl: './user.css'
})
export class User {
  @Input() user!:UserModel;
}
