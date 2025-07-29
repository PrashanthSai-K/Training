import { Component } from '@angular/core';
import { News, NewsService } from '../../Services/news.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-news-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './news-list.html',
  styleUrl: './news-list.css'
})
export class NewsList {
  newsList: News[] = [];
  currentPage: number = 1;
  pageCount: number = 1;

  constructor(private newsService: NewsService) {}

  ngOnInit(): void {
    this.loadNews(this.currentPage);
  }

  loadNews(page: number): void {
    this.newsService.getNews(page).subscribe(res => {
      this.newsList = res;
      // this.currentPage = res.pageNumber;
      // this.pageCount = res.pageCount;
    });
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.pageCount) {
      this.loadNews(page);
    }
  }

  get pageNumbers(): number[] {
    return Array(this.pageCount).fill(0).map((_, i) => i + 1);
  }

}
