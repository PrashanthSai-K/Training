import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { VideoUpload } from "./video-upload/video-upload";
import { VideoList } from "./video-list/video-list";
import { Navbar } from "./navbar/navbar";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, VideoUpload, VideoList, Navbar],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'streamapp';
}
