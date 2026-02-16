import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api-config';
import { SavedArticleItem, SavedArticleResult, UserProfile } from '../models/user';

@Injectable({ providedIn: 'root' })
export class UsersService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getProfile(userId: number): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.baseUrl}/users/${userId}/profile`);
  }

  getSavedArticles(
    userId: number,
    options?: { limit?: number; offset?: number },
  ): Observable<SavedArticleItem[]> {
    let params = new HttpParams();
    if (options?.limit != null) params = params.set('limit', options.limit);
    if (options?.offset != null) params = params.set('offset', options.offset);
    return this.http.get<SavedArticleItem[]>(
      `${this.baseUrl}/users/${userId}/saved`,
      { params },
    );
  }

  saveArticle(userId: number, articleId: number): Observable<SavedArticleResult> {
    return this.http.post<SavedArticleResult>(
      `${this.baseUrl}/users/${userId}/saved/${articleId}`,
      null,
    );
  }

  removeSavedArticle(userId: number, articleId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/users/${userId}/saved/${articleId}`,
    );
  }

  updateInterests(userId: number, categoryIds: number[]): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/users/${userId}/interests`,
      categoryIds,
    );
  }
}
