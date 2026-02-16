import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  CategoryPillComponent,
  FeaturedCardComponent,
  StandardCardComponent,
  TrendingCardComponent,
} from 'components';
import { FeedService, Article, Category } from 'api';

@Component({
  selector: 'app-home',
  imports: [
    CategoryPillComponent,
    FeaturedCardComponent,
    StandardCardComponent,
    TrendingCardComponent,
  ],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class HomeComponent implements OnInit {
  private readonly feedService = inject(FeedService);
  private readonly router = inject(Router);

  protected readonly categories = signal<Category[]>([]);
  protected readonly featured = signal<Article[]>([]);
  protected readonly latest = signal<Article[]>([]);
  protected readonly trending = signal<Article[]>([]);
  protected readonly activeCategory = signal('all');

  ngOnInit(): void {
    this.feedService.getFeed().subscribe((feed) => {
      this.featured.set(feed.featured);
      this.latest.set(feed.latest);
      this.trending.set(feed.trending);
      const cats = feed.categories.map((c) => c.category);
      this.categories.set(cats);
    });
  }

  protected onCategoryClick(slug: string): void {
    this.activeCategory.set(slug);
  }

  protected onArticleClick(id: number): void {
    this.router.navigate(['/article', id]);
  }

  protected timeAgo(dateStr: string | null): string {
    if (!dateStr) return '';
    const diff = Date.now() - new Date(dateStr).getTime();
    const hours = Math.floor(diff / 3600000);
    if (hours < 1) return 'Just now';
    if (hours < 24) return `${hours}h ago`;
    const days = Math.floor(hours / 24);
    if (days < 7) return `${days}d ago`;
    return `${Math.floor(days / 7)}w ago`;
  }
}
