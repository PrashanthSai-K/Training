import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-video-play',
  imports: [CommonModule],
  templateUrl: './video-play.html',
  styleUrl: './video-play.css'
})
export class VideoPlay {
  videoUrl: string = '';
  title: string = '';

  constructor(private route: ActivatedRoute, private http:HttpClient) { }

  ngOnInit(): void {
    const videoName = this.route.snapshot.paramMap.get('name');
    if (videoName) {
      // Replace this with your actual blob or backend video URL base
      this.http.get(`http://localhost:5283/api/file/stream/${videoName}`).subscribe({
        next: (res:any)=>{
          this.videoUrl = res.sasUrl;
        }
      })
      // this.videoUrl = `http://localhost:5283/api/file/stream/${videoName}`;
      this.title = decodeURIComponent(videoName.split('.')[0]);
    }
  }

}
