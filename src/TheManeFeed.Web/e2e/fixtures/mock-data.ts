export interface MockArticle {
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
  category: MockCategory | null;
  author: MockAuthor | null;
}

export interface MockCategory {
  id: number;
  name: string;
  slug: string;
  color: string;
  displayOrder: number;
}

export interface MockAuthor {
  id: number;
  name: string;
  avatarUrl: string | null;
  bio: string | null;
}

export interface MockTrendingSearch {
  id: number;
  query: string;
  searchCount: number;
}

export interface MockFeedResponse {
  featured: MockArticle[];
  latest: MockArticle[];
  trending: MockArticle[];
  categories: { category: MockCategory; articles: MockArticle[] }[];
}

export const categories: MockCategory[] = [
  { id: 1, name: 'Color Trends', slug: 'color', color: '#C85A7C', displayOrder: 1 },
  { id: 2, name: 'Celebrity Hair', slug: 'celebrity', color: '#C9A96E', displayOrder: 2 },
  { id: 3, name: 'Haircuts & Styles', slug: 'cuts', color: '#8B7D6B', displayOrder: 3 },
  { id: 4, name: 'Hair Care', slug: 'care', color: '#D4A0A7', displayOrder: 4 },
  { id: 5, name: 'Products & Reviews', slug: 'products', color: '#1A1A1A', displayOrder: 5 },
  { id: 6, name: 'Salon Guide', slug: 'salon', color: '#C9A96E', displayOrder: 6 },
];

export const authors: MockAuthor[] = [
  { id: 1, name: 'Maya Williams', avatarUrl: null, bio: 'Beauty editor' },
  { id: 2, name: 'Zoe Carter', avatarUrl: null, bio: 'Hair stylist' },
];

function makeArticle(overrides: Partial<MockArticle> & { id: number }): MockArticle {
  return {
    url: `https://example.com/article-${overrides.id}`,
    title: `Test Article ${overrides.id}`,
    summary: `Summary for article ${overrides.id}`,
    body: null,
    imageUrl: `https://picsum.photos/seed/${overrides.id}/400/300`,
    sourceName: 'TestSource',
    categoryId: null,
    authorId: null,
    publishedAt: new Date(Date.now() - overrides.id * 3600000).toISOString(),
    scrapedAt: new Date().toISOString(),
    readCount: 0,
    isFeatured: false,
    isTrending: false,
    category: null,
    author: null,
    ...overrides,
  };
}

export const featuredArticle: MockArticle = makeArticle({
  id: 1,
  title: 'Summer Hair Color Trends You Need to Try',
  summary: 'Discover the hottest hair color trends for the season.',
  sourceName: 'Essence',
  isFeatured: true,
  category: categories[0],
  author: authors[0],
});

export const latestArticles: MockArticle[] = [
  makeArticle({
    id: 2,
    title: 'Best Leave-In Conditioners for Natural Hair',
    summary: 'Top picks for moisturizing your curls.',
    sourceName: 'NaturallyCurly',
    category: categories[3],
    author: authors[1],
  }),
  makeArticle({
    id: 3,
    title: 'Celebrity Bob Cuts That Are Trending',
    summary: 'Stars rocking the bob look this season.',
    sourceName: 'TheCut',
    category: categories[1],
  }),
  makeArticle({
    id: 4,
    title: 'Protective Styles for Every Occasion',
    summary: 'Versatile styles to protect your hair.',
    sourceName: 'Byrdie',
    category: categories[2],
  }),
];

export const trendingArticles: MockArticle[] = [
  makeArticle({
    id: 5,
    title: 'The Rise of Silk Press',
    sourceName: 'Allure',
    isTrending: true,
    category: categories[2],
  }),
  makeArticle({
    id: 6,
    title: 'Box Braids: A Complete Guide',
    sourceName: 'Glamour',
    isTrending: true,
    category: categories[2],
  }),
  makeArticle({
    id: 7,
    title: 'Top 10 Wigs for Beginners',
    sourceName: 'WigsCom',
    isTrending: true,
    category: categories[4],
  }),
];

export const articleDetail: MockArticle = makeArticle({
  id: 1,
  title: 'Summer Hair Color Trends You Need to Try',
  summary: 'Discover the hottest hair color trends for the season.',
  body: '<p>This summer is all about bold color choices.</p><blockquote>Color is the ultimate form of self-expression.</blockquote><p>From honey blonde to deep burgundy, the options are endless.</p>',
  sourceName: 'Essence',
  isFeatured: true,
  category: categories[0],
  author: authors[0],
});

export const feedResponse: MockFeedResponse = {
  featured: [featuredArticle],
  latest: latestArticles,
  trending: trendingArticles,
  categories: categories.map((cat) => ({
    category: cat,
    articles: latestArticles.filter((a) => a.category?.id === cat.id),
  })),
};

export const trendingSearches: MockTrendingSearch[] = [
  { id: 1, query: 'Box braids', searchCount: 150 },
  { id: 2, query: 'Silk press', searchCount: 120 },
  { id: 3, query: 'Wig installation', searchCount: 90 },
  { id: 4, query: 'Natural hair care', searchCount: 80 },
];

export const searchResults: MockArticle[] = [
  makeArticle({
    id: 10,
    title: 'How to Do Box Braids at Home',
    sourceName: 'NaturallyCurly',
    category: categories[2],
  }),
  makeArticle({
    id: 11,
    title: 'Box Braid Styles for Every Length',
    sourceName: 'Essence',
    category: categories[2],
  }),
];

export const allArticles: MockArticle[] = [
  featuredArticle,
  ...latestArticles,
  ...trendingArticles,
];
