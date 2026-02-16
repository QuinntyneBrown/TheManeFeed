import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api-config';
import {
  AddSearchHistoryRequest,
  SearchArticleItem,
  SearchHistoryItem,
  SearchHistoryRecord,
  TrendingSearch,
} from '../models/search';

@Injectable({ providedIn: 'root' })
export class SearchService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  search(
    q: string,
    options?: { limit?: number; offset?: number },
  ): Observable<SearchArticleItem[]> {
    let params = new HttpParams().set('q', q);
    if (options?.limit != null) params = params.set('limit', options.limit);
    if (options?.offset != null) params = params.set('offset', options.offset);
    return this.http.get<SearchArticleItem[]>(`${this.baseUrl}/search`, { params });
  }

  getTrendingSearches(limit?: number): Observable<TrendingSearch[]> {
    let params = new HttpParams();
    if (limit != null) params = params.set('limit', limit);
    return this.http.get<TrendingSearch[]>(`${this.baseUrl}/search/trending`, { params });
  }

  getHistory(userId: number, limit?: number): Observable<SearchHistoryItem[]> {
    let params = new HttpParams();
    if (limit != null) params = params.set('limit', limit);
    return this.http.get<SearchHistoryItem[]>(
      `${this.baseUrl}/search/history/${userId}`,
      { params },
    );
  }

  addHistory(request: AddSearchHistoryRequest): Observable<SearchHistoryRecord> {
    return this.http.post<SearchHistoryRecord>(
      `${this.baseUrl}/search/history`,
      request,
    );
  }

  clearHistory(userId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/search/history/${userId}`);
  }
}
