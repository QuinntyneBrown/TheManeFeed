import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article } from '../models/article.model';

@Injectable({ providedIn: 'root' })
export class ArticleService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/articles';

  getAll(limit = 20): Observable<Article[]> {
    const params = new HttpParams().set('limit', limit);
    return this.http.get<Article[]>(this.baseUrl, { params });
  }

  getById(id: number): Observable<Article> {
    return this.http.get<Article>(`${this.baseUrl}/${id}`);
  }

  getFeatured(): Observable<Article[]> {
    return this.http.get<Article[]>(`${this.baseUrl}/featured`);
  }

  getTrending(): Observable<Article[]> {
    return this.http.get<Article[]>(`${this.baseUrl}/trending`);
  }

  search(query: string): Observable<Article[]> {
    const params = new HttpParams().set('q', query);
    return this.http.get<Article[]>(`${this.baseUrl}/search`, { params });
  }
}
