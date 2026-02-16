import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  SearchBarComponent,
  StandardCardComponent,
  CategoryPillComponent,
  IconComponent,
} from 'components';
import { SearchService, ArticleService, Article, TrendingSearch } from 'api';

@Component({
  selector: 'app-search',
  imports: [
    SearchBarComponent,
    StandardCardComponent,
    CategoryPillComponent,
    IconComponent,
  ],
  templateUrl: './search.html',
  styleUrl: './search.scss',
})
export class SearchComponent implements OnInit {
  private readonly searchService = inject(SearchService);
  private readonly articleService = inject(ArticleService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  protected readonly recentSearches = signal<string[]>([
    'Box braids styles 2025',
    'Best leave-in conditioners',
    'Protective styles for summer',
  ]);
  protected readonly trendingSearches = signal<TrendingSearch[]>([]);
  protected readonly topStories = signal<Article[]>([]);
  protected readonly searchResults = signal<Article[]>([]);
  protected readonly hasSearched = signal(false);

  ngOnInit(): void {
    this.searchService.getTrending().subscribe((t) => this.trendingSearches.set(t));
    this.articleService.getTrending().subscribe((a) => this.topStories.set(a.slice(0, 4)));

    const q = this.route.snapshot.queryParamMap.get('q');
    if (q) {
      this.onSearch(q);
    }
  }

  protected onSearch(query: string): void {
    if (!query.trim()) return;
    this.hasSearched.set(true);
    this.searchService.search(query).subscribe((results) =>
      this.searchResults.set(results)
    );
  }

  protected onTrendingClick(query: string): void {
    this.onSearch(query);
  }

  protected onArticleClick(id: number): void {
    this.router.navigate(['/article', id]);
  }

  protected clearRecent(): void {
    this.recentSearches.set([]);
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
