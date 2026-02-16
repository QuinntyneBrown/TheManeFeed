import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../api-config';
import { HomeFeed } from '../models/feed';

@Injectable({ providedIn: 'root' })
export class FeedService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  getHomeFeed(): Observable<HomeFeed> {
    return this.http.get<HomeFeed>(`${this.baseUrl}/feed`);
  }
}
