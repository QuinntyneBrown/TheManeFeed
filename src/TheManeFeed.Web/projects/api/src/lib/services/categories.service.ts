import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api-config';
import { CategoryArticleItem } from '../models/article';
import { Category } from '../models/category';

@Injectable({ providedIn: 'root' })
export class CategoriesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.baseUrl}/categories`);
  }

  getBySlug(slug: string): Observable<Category> {
    return this.http.get<Category>(`${this.baseUrl}/categories/${slug}`);
  }

  getArticles(
    slug: string,
    options?: { limit?: number; offset?: number },
  ): Observable<CategoryArticleItem[]> {
    let params = new HttpParams();
    if (options?.limit != null) params = params.set('limit', options.limit);
    if (options?.offset != null) params = params.set('offset', options.offset);
    return this.http.get<CategoryArticleItem[]>(
      `${this.baseUrl}/categories/${slug}/articles`,
      { params },
    );
  }
}
