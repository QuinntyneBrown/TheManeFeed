import { test, expect } from '@playwright/test';
import { SearchPage } from '../pages/search.page';
import { mockAllApis } from '../helpers/api-mocks';
import { trendingSearches, searchResults } from '../fixtures/mock-data';

test.describe('Search Page', () => {
  let search: SearchPage;

  test.beforeEach(async ({ page }) => {
    await mockAllApis(page);
    search = new SearchPage(page);
  });

  test.describe('Default View', () => {
    test.beforeEach(async () => {
      await search.goto();
    });

    test('should display "Search" title', async () => {
      await search.expectLoaded();
    });

    test('should display search bar', async () => {
      await search.expectLoaded();
      await expect(search.searchBar).toBeVisible();
    });

    test('should display recent searches', async () => {
      await search.expectLoaded();
      await search.expectDefaultViewVisible();
      const texts = await search.getRecentSearchTexts();
      expect(texts.length).toBe(3);
      expect(texts).toContain('Box braids styles 2025');
      expect(texts).toContain('Best leave-in conditioners');
      expect(texts).toContain('Protective styles for summer');
    });

    test('should display trending searches as pills', async () => {
      await search.expectLoaded();
      const labels = await search.getTrendingPillLabels();
      for (const ts of trendingSearches) {
        expect(labels).toContain(ts.query);
      }
    });

    test('should display top stories', async () => {
      await search.expectLoaded();
      const count = await search.getTopStoriesCount();
      expect(count).toBeGreaterThan(0);
    });
  });

  test.describe('Clearing Recent Searches', () => {
    test('should clear recent searches on clear button click', async () => {
      await search.goto();
      await search.expectLoaded();
      await search.clearRecentSearches();
      await expect(search.recentSection).not.toBeVisible();
    });
  });

  test.describe('Performing a Search', () => {
    test('should show results when searching via search bar', async () => {
      await search.goto();
      await search.expectLoaded();
      await search.search('box braids');
      await search.expectResultsVisible();
      const count = await search.getResultsCount();
      expect(count).toBe(searchResults.length);
    });

    test('should hide default view when results are shown', async () => {
      await search.goto();
      await search.expectLoaded();
      await search.search('box braids');
      await search.expectResultsVisible();
      await expect(search.recentSection).not.toBeVisible();
      await expect(search.trendingSearchSection).not.toBeVisible();
    });

    test('should perform search when clicking a recent search', async () => {
      await search.goto();
      await search.expectLoaded();
      await search.clickRecentSearch(0);
      await search.expectResultsVisible();
    });

    test('should perform search when clicking a trending pill', async () => {
      await search.goto();
      await search.expectLoaded();
      await search.clickTrendingPill(trendingSearches[0].query);
      await search.expectResultsVisible();
    });

    test('should navigate to article on result card click', async ({ page }) => {
      await search.goto();
      await search.expectLoaded();
      await search.search('box braids');
      await search.expectResultsVisible();
      await search.clickResultCard(0);
      await expect(page).toHaveURL(/\/article\/\d+/);
    });
  });

  test.describe('Query Parameter Handling', () => {
    test('should auto-search when navigated with q param', async () => {
      await search.gotoWithQuery('box braids');
      await search.expectResultsVisible();
      const count = await search.getResultsCount();
      expect(count).toBe(searchResults.length);
    });
  });
});
