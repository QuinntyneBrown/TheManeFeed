export interface Article {
  id: number;
  url: string;
  title: string;
  summary: string;
  body: string | null;
  imageUrl: string | null;
  sourceName: string;
  categoryId: number | null;
  authorId: number | null;
  publishedAt: string | null;
  scrapedAt: string;
  readCount: number;
  isFeatured: boolean;
  isTrending: boolean;
  category: Category | null;
  author: Author | null;
}

export interface Category {
  id: number;
  name: string;
  slug: string;
  color: string;
  displayOrder: number;
}

export interface Author {
  id: number;
  name: string;
  avatarUrl: string | null;
  bio: string | null;
}

export interface User {
  id: number;
  displayName: string;
  username: string;
  email: string;
  avatarUrl: string | null;
  createdAt: string;
}

export interface UserProfile {
  user: User;
  savedCount: number;
  interestCount: number;
  collectionCount: number;
  interests: Category[];
}

export interface SavedArticle {
  id: number;
  userId: number;
  articleId: number;
  savedAt: string;
  article: Article;
}

export interface Collection {
  id: number;
  userId: number;
  name: string;
  description: string | null;
  createdAt: string;
  articles: Article[];
}

export interface SearchHistory {
  id: number;
  query: string;
  searchedAt: string;
}

export interface TrendingSearch {
  id: number;
  query: string;
  searchCount: number;
}

export interface FeedResponse {
  featured: Article[];
  latest: Article[];
  trending: Article[];
  categories: Category[];
}

export interface CategoryWithArticles {
  category: Category;
  articles: Article[];
}
