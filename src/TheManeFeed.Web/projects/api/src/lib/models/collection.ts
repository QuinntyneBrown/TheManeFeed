export interface CollectionListItem {
  id: number;
  name: string;
  description: string | null;
  createdAt: string;
  articleCount: number;
}

export interface CollectionArticleSummary {
  id: number;
  title: string;
  summary: string | null;
  imageUrl: string | null;
  sourceName: string;
}

export interface CollectionArticleEntry {
  addedAt: string;
  article: CollectionArticleSummary;
}

export interface CollectionDetail {
  id: number;
  name: string;
  description: string | null;
  createdAt: string;
  articles: CollectionArticleEntry[];
}

export interface CollectionCreated {
  id: number;
  name: string;
  description: string | null;
  createdAt: string;
}

export interface CreateCollectionRequest {
  userId: number;
  name: string;
  description?: string | null;
}

export interface UpdateCollectionRequest {
  name: string;
  description?: string | null;
}
