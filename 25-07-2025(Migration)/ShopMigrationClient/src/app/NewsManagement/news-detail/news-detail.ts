import { Component } from '@angular/core';
import { News, NewsService } from '../../Services/news.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-news-detail',
  imports: [CommonModule],
  templateUrl: './news-detail.html',
  styleUrl: './news-detail.css'
})
export class NewsDetail {
  news?: News;

  constructor(private route: ActivatedRoute, private newsService: NewsService) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.newsService.getById(+id).subscribe(n => this.news = n);
    }
  }

}
