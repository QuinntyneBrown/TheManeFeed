import { Page } from '@playwright/test';
import {
  feedResponse,
  categories,
  articleDetail,
  allArticles,
  trendingArticles,
  trendingSearches,
  searchResults,
} from '../fixtures/mock-data';

export async function mockAllApis(page: Page): Promise<void> {
  await mockFeedApi(page);
  await mockArticlesApi(page);
  await mockCategoriesApi(page);
  await mockSearchApi(page);
  await mockUsersApi(page);
}

export async function mockFeedApi(page: Page): Promise<void> {
  await page.route('**/api/feed', (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(feedResponse),
    });
  });
}

export async function mockArticlesApi(page: Page): Promise<void> {
  await page.route('**/api/articles/featured', (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(feedResponse.featured),
    });
  });

  await page.route('**/api/articles/trending', (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(trendingArticles),
    });
  });

  await page.route('**/api/articles/search*', (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(searchResults),
    });
  });

  await page.route(/\/api\/articles\/\d+$/, (route) => {
    const url = route.request().url();
    const id = parseInt(url.split('/').pop()!, 10);
    const article = allArticles.find((a) => a.id === id) ?? articleDetail;
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({ ...article, id }),
    });
  });

  await page.route('**/api/articles?*', (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(allArticles.slice(0, 4)),
    });
  });

  await page.route('**/api/articles', (route) => {
    if (route.request().url().includes('?')) return;
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(allArticles),
    });
  });
}

export async function mockCategoriesApi(page: Page): Promise<void> {
  await page.route('**/api/categories', (route) => {
    if (route.request().url().includes('/api/categories/')) return;
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(categories),
    });
  });

  await page.route(/\/api\/categories\/[\w-]+\/articles/, (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(allArticles.slice(0, 2)),
    });
  });

  await page.route(/\/api\/categories\/[\w-]+$/, (route) => {
    const slug = route.request().url().split('/').pop()!;
    const cat = categories.find((c) => c.slug === slug) ?? categories[0];
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(cat),
    });
  });
}

export async function mockSearchApi(page: Page): Promise<void> {
  await page.route('**/api/search/trending', (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(trendingSearches),
    });
  });

  await page.route('**/api/search/history/*', (route) => {
    if (route.request().method() === 'DELETE') {
      route.fulfill({ status: 204 });
      return;
    }
    if (route.request().method() === 'POST') {
      route.fulfill({ status: 201 });
      return;
    }
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify([]),
    });
  });

  await page.route('**/api/search?*', (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(searchResults),
    });
  });
}

export async function mockUsersApi(page: Page): Promise<void> {
  await page.route(/\/api\/users\/\d+\/profile/, (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        user: {
          id: 1,
          displayName: 'Amara Johnson',
          username: 'amaraj',
          email: 'amara@example.com',
          avatarUrl: null,
          createdAt: new Date().toISOString(),
        },
        savedCount: 24,
        interestCount: 6,
        collectionCount: 3,
        interests: categories.slice(0, 4),
      }),
    });
  });

  await page.route(/\/api\/users\/\d+\/saved/, (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify([]),
    });
  });

  await page.route(/\/api\/users\/\d+\/interests/, (route) => {
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify(categories.slice(0, 4)),
    });
  });
}
