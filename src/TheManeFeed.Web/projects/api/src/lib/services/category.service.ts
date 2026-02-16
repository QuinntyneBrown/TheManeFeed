import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article, Category } from '../models/article.model';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/categories';

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl);
  }

  getBySlug(slug: string): Observable<Category> {
    return this.http.get<Category>(`${this.baseUrl}/${slug}`);
  }

  getArticles(slug: string): Observable<Article[]> {
    return this.http.get<Article[]>(`${this.baseUrl}/${slug}/articles`);
  }
}
