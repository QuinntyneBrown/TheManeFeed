import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IconComponent, StandardCardComponent } from 'components';
import { ArticleService, Article } from 'api';

@Component({
  selector: 'app-article-detail',
  imports: [IconComponent, StandardCardComponent],
  templateUrl: './article-detail.html',
  styleUrl: './article-detail.scss',
})
export class ArticleDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly articleService = inject(ArticleService);

  protected readonly article = signal<Article | null>(null);
  protected readonly relatedArticles = signal<Article[]>([]);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.articleService.getById(id).subscribe((a) => this.article.set(a));
      this.articleService.getAll(4).subscribe((articles) =>
        this.relatedArticles.set(articles.filter((a) => a.id !== id).slice(0, 3))
      );
    }
  }

  protected goBack(): void {
    this.router.navigateByUrl('/');
  }

  protected onRelatedClick(id: number): void {
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

  protected formatDate(dateStr: string | null): string {
    if (!dateStr) return '';
    return new Date(dateStr).toLocaleDateString('en-US', {
      month: 'long',
      day: 'numeric',
      year: 'numeric',
    });
  }
}
