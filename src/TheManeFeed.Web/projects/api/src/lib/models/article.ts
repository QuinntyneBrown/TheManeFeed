export interface ArticleListItem {
  id: number;
  title: string;
  summary: string | null;
  imageUrl: string | null;
  sourceName: string;
  url: string;
  category: string | null;
  categorySlug: string | null;
  author: string | null;
  publishedAt: string | null;
  readCount: number;
  isFeatured: boolean;
  isTrending: boolean;
}

export interface ArticleAuthor {
  id: number;
  name: string;
  avatarUrl: string | null;
  bio: string | null;
}

export interface ArticleDetail {
  id: number;
  title: string;
  summary: string | null;
  body: string | null;
  imageUrl: string | null;
  sourceName: string;
  url: string;
  category: string | null;
  categorySlug: string | null;
  author: ArticleAuthor | null;
  publishedAt: string | null;
  scrapedAt: string;
  readCount: number;
  isFeatured: boolean;
  isTrending: boolean;
}

export interface CategoryArticleItem {
  id: number;
  title: string;
  summary: string | null;
  imageUrl: string | null;
  sourceName: string;
  url: string;
  author: string | null;
  publishedAt: string | null;
  readCount: number;
}
