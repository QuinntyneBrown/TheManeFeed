import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api-config';
import {
  CollectionCreated,
  CollectionDetail,
  CollectionListItem,
  CreateCollectionRequest,
  UpdateCollectionRequest,
} from '../models/collection';

@Injectable({ providedIn: 'root' })
export class CollectionsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getByUser(userId: number): Observable<CollectionListItem[]> {
    return this.http.get<CollectionListItem[]>(
      `${this.baseUrl}/collections/user/${userId}`,
    );
  }

  getById(id: number): Observable<CollectionDetail> {
    return this.http.get<CollectionDetail>(`${this.baseUrl}/collections/${id}`);
  }

  create(request: CreateCollectionRequest): Observable<CollectionCreated> {
    return this.http.post<CollectionCreated>(`${this.baseUrl}/collections`, request);
  }

  update(id: number, request: UpdateCollectionRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/collections/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/collections/${id}`);
  }

  addArticle(collectionId: number, articleId: number): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/collections/${collectionId}/articles/${articleId}`,
      null,
    );
  }

  removeArticle(collectionId: number, articleId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/collections/${collectionId}/articles/${articleId}`,
    );
  }
}
