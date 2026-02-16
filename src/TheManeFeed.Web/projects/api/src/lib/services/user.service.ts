import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category, SavedArticle, UserProfile } from '../models/article.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/users';

  getProfile(userId: number): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.baseUrl}/${userId}/profile`);
  }

  getSavedArticles(userId: number): Observable<SavedArticle[]> {
    return this.http.get<SavedArticle[]>(`${this.baseUrl}/${userId}/saved`);
  }

  saveArticle(userId: number, articleId: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${userId}/saved/${articleId}`, {});
  }

  unsaveArticle(userId: number, articleId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${userId}/saved/${articleId}`);
  }

  updateInterests(userId: number, categoryIds: number[]): Observable<Category[]> {
    return this.http.put<Category[]>(`${this.baseUrl}/${userId}/interests`, categoryIds);
  }
}
