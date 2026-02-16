import { test, expect } from '@playwright/test';
import { ArticleDetailPage } from '../pages/article-detail.page';
import { mockAllApis } from '../helpers/api-mocks';
import { articleDetail } from '../fixtures/mock-data';

test.describe('Article Detail Page', () => {
  let detail: ArticleDetailPage;

  test.beforeEach(async ({ page }) => {
    await mockAllApis(page);
    detail = new ArticleDetailPage(page);
    await detail.goto(articleDetail.id);
  });

  test.describe('Top Bar', () => {
    test('should display back, share, and bookmark buttons', async () => {
      await detail.expectLoaded();
      await expect(detail.backButton).toBeVisible();
      await expect(detail.shareButton).toBeVisible();
      await expect(detail.bookmarkButton).toBeVisible();
    });

    test('should navigate back to home on back button click', async ({ page }) => {
      await detail.expectLoaded();
      await detail.clickBack();
      await expect(page).toHaveURL('/');
    });
  });

  test.describe('Hero Image', () => {
    test('should display hero image when article has imageUrl', async () => {
      await detail.expectLoaded();
      expect(await detail.hasHeroImage()).toBe(true);
    });
  });

  test.describe('Article Content', () => {
    test('should display the article title', async () => {
      await detail.expectLoaded();
      const title = await detail.getTitle();
      expect(title).toBe(articleDetail.title);
    });

    test('should display the category label', async () => {
      await detail.expectLoaded();
      const category = await detail.getCategory();
      expect(category).toBe(articleDetail.category!.name);
    });

    test('should display the author name', async () => {
      await detail.expectLoaded();
      const name = await detail.getAuthorName();
      expect(name).toBe(articleDetail.author!.name);
    });

    test('should display the publication date', async () => {
      await detail.expectLoaded();
      const date = await detail.getDate();
      expect(date).toBeTruthy();
    });

    test('should display article body content', async () => {
      await detail.expectLoaded();
      expect(await detail.hasBody()).toBe(true);
      const bodyText = await detail.getBodyText();
      expect(bodyText).toContain('bold color choices');
    });
  });

  test.describe('Related Stories', () => {
    test('should display related articles section', async () => {
      await detail.expectLoaded();
      await expect(detail.relatedTitle).toHaveText('Related Stories');
    });

    test('should display related article cards', async () => {
      await detail.expectLoaded();
      const count = await detail.getRelatedCardCount();
      expect(count).toBeGreaterThan(0);
    });

    test('should navigate to another article on related card click', async ({ page }) => {
      await detail.expectLoaded();
      const count = await detail.getRelatedCardCount();
      if (count > 0) {
        await detail.clickRelatedCard(0);
        await expect(page).toHaveURL(/\/article\/\d+/);
      }
    });
  });
});
