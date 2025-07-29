import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { News, NewsService } from '../../Services/news.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-news-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './news-form.html',
  styleUrl: './news-form.css'
})
export class NewsForm {
  form!: FormGroup;
  isEdit = false;
  id?: number;
  users:any;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private newsService: NewsService,
    private router: Router,
    private http:HttpClient
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      newsId: [0],
      title: ['', Validators.required],
      shortDescription: [''],
      image: [''],
      content: [''],
      createdDate: ['', Validators.required],
      status: [1],
      userId: [0, Validators.required]
    });
    this.http.get<any[]>('http://localhost:5038/api/user').subscribe(data => this.users = data);

    this.id = +this.route.snapshot.paramMap.get('id')!;
    if (this.id) {
      this.isEdit = true;
      this.newsService.getById(this.id).subscribe(news => {
        news.createdDate = new Date(news.createdDate).toISOString().substring(0, 16); // 'yyyy-MM-ddTHH:mm'
        this.form.patchValue(news);
      });
    }
  }

  save() {
    const news: News = this.form.value;
    news.createdDate = new Date(news.createdDate).toISOString();

    if (this.isEdit) {
      this.newsService.update(news.newsId!, news).subscribe(() => {
        this.router.navigate(['/news']);
      });
    } else {
      this.newsService.create(news).subscribe(() => {
        this.router.navigate(['/news']);
      });
    }
  }
}
