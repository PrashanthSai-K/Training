import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-first',
  imports: [FormsModule],
  templateUrl: './first.html',
  styleUrl: './first.css'
})
export class First {
    name : string;
    classname:string = "bi bi-balloon-heart";
    like:boolean = false;
    constructor()
    {
      this.name = "Yokosoo"
    }
    onButtonClick(uname:string){
        this.name = uname;
    }
    onToggleLike()
    {
      this.like = !this.like;
      if(this.like)
          this.classname = "bi bi-balloon-heart-fill";
      else
          this.classname = "bi bi-balloon-heart";
    }
}
