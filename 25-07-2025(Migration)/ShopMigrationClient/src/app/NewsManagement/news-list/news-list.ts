import { Component } from '@angular/core';
import { News, NewsService } from '../../Services/news.service';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-news-list',
  imports: [RouterLink, CommonModule],
  templateUrl: './news-list.html',
  styleUrl: './news-list.css'
})
export class NewsList {
  newsList: News[] = [];

  constructor(private newsService: NewsService) {}

  ngOnInit() {
    this.newsService.getAll().subscribe(data => this.newsList = data);
  }

  deleteNews(id: number) {
    if (confirm('Are you sure?')) {
      this.newsService.delete(id).subscribe(() => {
        this.newsList = this.newsList.filter(n => n.newsId !== id);
      });
    }
  }

}
