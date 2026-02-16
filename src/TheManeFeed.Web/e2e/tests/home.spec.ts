import { test, expect } from '@playwright/test';
import { HomePage } from '../pages/home.page';
import { mockAllApis } from '../helpers/api-mocks';
import { feedResponse, featuredArticle, latestArticles, trendingArticles } from '../fixtures/mock-data';

test.describe('Home Page', () => {
  let home: HomePage;

  test.beforeEach(async ({ page }) => {
    await mockAllApis(page);
    home = new HomePage(page);
    await home.goto();
  });

  test.describe('Category Pills', () => {
    test('should display "All" pill and category pills from feed', async () => {
      await home.expectLoaded();
      const labels = await home.getCategoryLabels();
      expect(labels.map((l) => l.trim())).toContain('All');
      for (const cat of feedResponse.categories) {
        expect(labels.map((l) => l.trim())).toContain(cat.category.name);
      }
    });

    test('should have "All" active by default', async () => {
      await home.expectLoaded();
      const active = await home.getActiveCategoryLabel();
      expect(active).toBe('All');
    });

    test('should switch active category on click', async () => {
      await home.expectLoaded();
      const catName = feedResponse.categories[0].category.name;
      await home.clickCategory(catName);
      const active = await home.getActiveCategoryLabel();
      expect(active).toBe(catName);
    });
  });

  test.describe('Featured Card', () => {
    test('should display the featured article', async () => {
      await home.expectLoaded();
      const title = await home.getFeaturedTitle();
      expect(title).toBe(featuredArticle.title);
    });

    test('should navigate to article detail on featured card click', async ({ page }) => {
      await home.expectLoaded();
      await home.clickFeaturedCard();
      await expect(page).toHaveURL(`/article/${featuredArticle.id}`);
    });
  });

  test.describe('Latest Stories', () => {
    test('should display section title', async () => {
      await home.expectLoaded();
      await expect(home.latestTitle).toHaveText('Latest Stories');
    });

    test('should display latest article cards', async () => {
      await home.expectLoaded();
      const titles = await home.getLatestCardTitles();
      expect(titles.length).toBe(latestArticles.length);
      for (const article of latestArticles) {
        expect(titles).toContain(article.title);
      }
    });

    test('should navigate to article detail on latest card click', async ({ page }) => {
      await home.expectLoaded();
      await home.clickLatestCard(0);
      await expect(page).toHaveURL(`/article/${latestArticles[0].id}`);
    });
  });

  test.describe('Trending Section', () => {
    test('should display section title', async () => {
      await home.expectLoaded();
      await expect(home.trendingTitle).toHaveText('Trending Now');
    });

    test('should display trending articles with ranks', async () => {
      await home.expectLoaded();
      const titles = await home.getTrendingCardTitles();
      expect(titles.length).toBe(trendingArticles.length);
      for (const article of trendingArticles) {
        expect(titles).toContain(article.title);
      }
    });

    test('should display sequential rank numbers', async () => {
      await home.expectLoaded();
      const ranks = await home.getTrendingRanks();
      expect(ranks).toEqual(['01', '02', '03']);
    });

    test('should navigate to article detail on trending card click', async ({ page }) => {
      await home.expectLoaded();
      await home.clickTrendingCard(0);
      await expect(page).toHaveURL(`/article/${trendingArticles[0].id}`);
    });
  });
});
