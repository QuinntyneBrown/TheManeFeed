import { ArticleListItem } from './article';

export interface FeedCategory {
  id: number;
  name: string;
  slug: string;
  color: string | null;
}

export interface TrendingFeedItem {
  id: number;
  title: string;
  readCount: number;
  isTrending: boolean;
  category: string | null;
}

export interface HomeFeed {
  categories: FeedCategory[];
  featured: ArticleListItem[];
  latest: ArticleListItem[];
  trending: TrendingFeedItem[];
}
