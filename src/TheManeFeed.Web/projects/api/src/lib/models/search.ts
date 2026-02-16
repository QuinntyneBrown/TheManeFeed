export interface SearchArticleItem {
  id: number;
  title: string;
  summary: string | null;
  imageUrl: string | null;
  sourceName: string;
  category: string | null;
  publishedAt: string | null;
}

export interface TrendingSearch {
  query: string;
  searchCount: number;
}

export interface SearchHistoryItem {
  query: string;
  searchedAt: string;
}

export interface SearchHistoryRecord {
  id: number;
  query: string;
  searchedAt: string;
}

export interface AddSearchHistoryRequest {
  userId: number;
  query: string;
}
