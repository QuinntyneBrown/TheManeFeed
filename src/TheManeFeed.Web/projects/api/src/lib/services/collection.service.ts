import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Collection } from '../models/article.model';

@Injectable({ providedIn: 'root' })
export class CollectionService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/collections';

  getByUser(userId: number): Observable<Collection[]> {
    return this.http.get<Collection[]>(`${this.baseUrl}/user/${userId}`);
  }

  getById(id: number): Observable<Collection> {
    return this.http.get<Collection>(`${this.baseUrl}/${id}`);
  }

  create(collection: Partial<Collection>): Observable<Collection> {
    return this.http.post<Collection>(this.baseUrl, collection);
  }

  update(id: number, collection: Partial<Collection>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, collection);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  addArticle(collectionId: number, articleId: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${collectionId}/articles/${articleId}`, {});
  }

  removeArticle(collectionId: number, articleId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${collectionId}/articles/${articleId}`);
  }
}
