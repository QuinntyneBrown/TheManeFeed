import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { SearchBarComponent, StandardCardComponent } from 'components';
import { CategoryService, ArticleService, Category, Article } from 'api';

@Component({
  selector: 'app-explore',
  imports: [SearchBarComponent, StandardCardComponent],
  templateUrl: './explore.html',
  styleUrl: './explore.scss',
})
export class ExploreComponent implements OnInit {
  private readonly categoryService = inject(CategoryService);
  private readonly articleService = inject(ArticleService);
  private readonly router = inject(Router);

  protected readonly categories = signal<Category[]>([]);
  protected readonly popular = signal<Article[]>([]);

  ngOnInit(): void {
    this.categoryService.getAll().subscribe((cats) => this.categories.set(cats));
    this.articleService.getTrending().subscribe((articles) =>
      this.popular.set(articles.slice(0, 4))
    );
  }

  protected onSearch(query: string): void {
    if (query.trim()) {
      this.router.navigate(['/search'], { queryParams: { q: query } });
    }
  }

  protected onCategoryClick(slug: string): void {
    // Navigate to explore with category filter (future enhancement)
    this.router.navigate(['/explore'], { queryParams: { category: slug } });
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
