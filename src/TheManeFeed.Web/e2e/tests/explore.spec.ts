import { test, expect } from '@playwright/test';
import { ExplorePage } from '../pages/explore.page';
import { mockAllApis } from '../helpers/api-mocks';
import { categories } from '../fixtures/mock-data';

test.describe('Explore Page', () => {
  let explore: ExplorePage;

  test.beforeEach(async ({ page }) => {
    await mockAllApis(page);
    explore = new ExplorePage(page);
    await explore.goto();
  });

  test.describe('Page Header', () => {
    test('should display "Explore" title', async () => {
      await explore.expectLoaded();
    });

    test('should display search bar', async () => {
      await explore.expectLoaded();
      await expect(explore.searchBar).toBeVisible();
    });
  });

  test.describe('Search', () => {
    test('should navigate to search page with query on search', async ({ page }) => {
      await explore.expectLoaded();
      await explore.search('box braids');
      await expect(page).toHaveURL(/\/search\?q=box%20braids/);
    });
  });

  test.describe('Browse Categories', () => {
    test('should display section title', async () => {
      await explore.expectLoaded();
      await expect(explore.categoriesTitle).toHaveText('Browse Categories');
    });

    test('should display category cards matching API data', async () => {
      await explore.expectLoaded();
      const names = await explore.getCategoryNames();
      expect(names.length).toBe(categories.length);
      for (const cat of categories) {
        expect(names).toContain(cat.name);
      }
    });

    test('should render categories in a grid layout', async ({ page }) => {
      await explore.expectLoaded();
      const grid = page.locator('.category-grid');
      await expect(grid).toBeVisible();
      const count = await explore.getCategoryCount();
      expect(count).toBeGreaterThanOrEqual(4);
    });

    test('should navigate on category click', async ({ page }) => {
      await explore.expectLoaded();
      await explore.clickCategory(categories[0].name);
      await expect(page).toHaveURL(/\/explore\?category=/);
    });
  });

  test.describe('Popular This Week', () => {
    test('should display section title', async () => {
      await explore.expectLoaded();
      await expect(explore.popularTitle).toHaveText('Popular This Week');
    });

    test('should display popular article cards', async () => {
      await explore.expectLoaded();
      const count = await explore.getPopularCardCount();
      expect(count).toBeGreaterThan(0);
    });

    test('should navigate to article on popular card click', async ({ page }) => {
      await explore.expectLoaded();
      await explore.clickPopularCard(0);
      await expect(page).toHaveURL(/\/article\/\d+/);
    });
  });
});
