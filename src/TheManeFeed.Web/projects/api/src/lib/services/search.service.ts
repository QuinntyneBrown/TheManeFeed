import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article, SearchHistory, TrendingSearch } from '../models/article.model';

@Injectable({ providedIn: 'root' })
export class SearchService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/search';

  search(query: string): Observable<Article[]> {
    const params = new HttpParams().set('q', query);
    return this.http.get<Article[]>(this.baseUrl, { params });
  }

  getTrending(): Observable<TrendingSearch[]> {
    return this.http.get<TrendingSearch[]>(`${this.baseUrl}/trending`);
  }

  getHistory(userId: number): Observable<SearchHistory[]> {
    return this.http.get<SearchHistory[]>(`${this.baseUrl}/history/${userId}`);
  }

  addHistory(userId: number, query: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/history/${userId}`, { query });
  }

  clearHistory(userId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/history/${userId}`);
  }
}
