import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api-config';
import { ArticleDetail, ArticleListItem } from '../models/article';

@Injectable({ providedIn: 'root' })
export class ArticlesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getArticles(options?: {
    categoryId?: number;
    source?: string;
    limit?: number;
    offset?: number;
  }): Observable<ArticleListItem[]> {
    let params = new HttpParams();
    if (options?.categoryId != null) params = params.set('categoryId', options.categoryId);
    if (options?.source) params = params.set('source', options.source);
    if (options?.limit != null) params = params.set('limit', options.limit);
    if (options?.offset != null) params = params.set('offset', options.offset);
    return this.http.get<ArticleListItem[]>(`${this.baseUrl}/articles`, { params });
  }

  getArticle(id: number): Observable<ArticleDetail> {
    return this.http.get<ArticleDetail>(`${this.baseUrl}/articles/${id}`);
  }

  getFeatured(limit?: number): Observable<ArticleListItem[]> {
    let params = new HttpParams();
    if (limit != null) params = params.set('limit', limit);
    return this.http.get<ArticleListItem[]>(`${this.baseUrl}/articles/featured`, { params });
  }

  getTrending(limit?: number): Observable<ArticleListItem[]> {
    let params = new HttpParams();
    if (limit != null) params = params.set('limit', limit);
    return this.http.get<ArticleListItem[]>(`${this.baseUrl}/articles/trending`, { params });
  }

  search(
    q: string,
    options?: { limit?: number; offset?: number },
  ): Observable<ArticleListItem[]> {
    let params = new HttpParams().set('q', q);
    if (options?.limit != null) params = params.set('limit', options.limit);
    if (options?.offset != null) params = params.set('offset', options.offset);
    return this.http.get<ArticleListItem[]>(`${this.baseUrl}/articles/search`, { params });
  }
}
