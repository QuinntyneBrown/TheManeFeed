import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FeedResponse } from '../models/article.model';

@Injectable({ providedIn: 'root' })
export class FeedService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/feed';

  getFeed(): Observable<FeedResponse> {
    return this.http.get<FeedResponse>(this.baseUrl);
  }
}
