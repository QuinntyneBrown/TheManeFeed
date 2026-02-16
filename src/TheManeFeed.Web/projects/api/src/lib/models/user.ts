export interface UserInterestCategory {
  id: number;
  name: string;
  slug: string;
  color: string | null;
}

export interface UserInterest {
  category: UserInterestCategory;
}

export interface UserStats {
  saved: number;
  topics: number;
  collections: number;
}

export interface UserProfile {
  id: number;
  displayName: string;
  username: string;
  email: string;
  avatarUrl: string | null;
  stats: UserStats;
  interests: UserInterest[];
}

export interface SavedArticleSummary {
  id: number;
  title: string;
  summary: string | null;
  imageUrl: string | null;
  sourceName: string;
  category: string | null;
  publishedAt: string | null;
}

export interface SavedArticleItem {
  savedAt: string;
  article: SavedArticleSummary;
}

export interface SavedArticleResult {
  id: number;
  savedAt: string;
}
