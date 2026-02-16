/*
 * Public API Surface of api
 */

// Configuration
export { API_BASE_URL } from './lib/api-config';

// Models
export type {
  ArticleListItem,
  ArticleAuthor,
  ArticleDetail,
  CategoryArticleItem,
} from './lib/models/article';
export type { Category } from './lib/models/category';
export type {
  CollectionListItem,
  CollectionArticleSummary,
  CollectionArticleEntry,
  CollectionDetail,
  CollectionCreated,
  CreateCollectionRequest,
  UpdateCollectionRequest,
} from './lib/models/collection';
export type { FeedCategory, TrendingFeedItem, HomeFeed } from './lib/models/feed';
export type {
  SearchArticleItem,
  TrendingSearch,
  SearchHistoryItem,
  SearchHistoryRecord,
  AddSearchHistoryRequest,
} from './lib/models/search';
export type {
  UserInterestCategory,
  UserInterest,
  UserStats,
  UserProfile,
  SavedArticleSummary,
  SavedArticleItem,
  SavedArticleResult,
} from './lib/models/user';

// Services
export { ArticlesService } from './lib/services/articles.service';
export { CategoriesService } from './lib/services/categories.service';
export { CollectionsService } from './lib/services/collections.service';
export { FeedService } from './lib/services/feed.service';
export { SearchService } from './lib/services/search.service';
export { UsersService } from './lib/services/users.service';
