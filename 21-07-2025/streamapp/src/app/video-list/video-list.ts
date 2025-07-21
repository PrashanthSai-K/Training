import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface Video {
  id: string;
  title: string;
  description: string;
  blobUrl: string;
}

@Component({
  selector: 'app-video-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './video-list.html',
  styleUrl: './video-list.css'
})
export class VideoList {
  videos: Video[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get<Video[]>('http://localhost:5283/api/file')
      .subscribe(data => {
        this.videos = data;
      });
  }

}
