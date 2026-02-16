import { test, expect } from '@playwright/test';
import { AppLayoutPage } from '../pages/app-layout.page';
import { mockAllApis } from '../helpers/api-mocks';

test.describe('Navigation', () => {
  let layout: AppLayoutPage;

  test.beforeEach(async ({ page }) => {
    await mockAllApis(page);
    layout = new AppLayoutPage(page);
  });

  test.describe('Mobile Navigation', () => {
    test.use({ viewport: { width: 375, height: 812 } });

    test('should display mobile header and bottom nav', async ({ page }) => {
      await page.goto('/');
      await layout.expectMobileHeaderVisible();
      await layout.expectBottomNavVisible();
    });

    test('should navigate to search on mobile search icon click', async ({ page }) => {
      await page.goto('/');
      await layout.clickMobileSearch();
      await expect(page).toHaveURL('/search');
    });

    test('should navigate to profile on mobile notification icon click', async ({ page }) => {
      await page.goto('/');
      await layout.clickMobileNotification();
      await expect(page).toHaveURL('/profile');
    });

    test('should navigate via bottom nav tabs', async ({ page }) => {
      await page.goto('/');

      await layout.navigateToTab('Explore');
      await expect(page).toHaveURL('/explore');

      await layout.navigateToTab('Trending');
      await expect(page).toHaveURL('/search');

      await layout.navigateToTab('Saved');
      await expect(page).toHaveURL('/profile');

      await layout.navigateToTab('Home');
      await expect(page).toHaveURL('/');
    });

    test('should highlight active bottom nav tab', async ({ page }) => {
      await page.goto('/');
      expect(await layout.getActiveTab()).toContain('Home');

      await layout.navigateToTab('Explore');
      expect(await layout.getActiveTab()).toContain('Explore');
    });
  });

  test.describe('Desktop Navigation', () => {
    test.use({ viewport: { width: 1280, height: 800 } });

    test('should display desktop header and hide mobile elements', async ({ page }) => {
      await page.goto('/');
      await expect(layout.desktopHeader).toBeVisible();
      await expect(layout.mobileBottomNav).not.toBeVisible();
    });

    test('should navigate via desktop nav links', async ({ page }) => {
      await page.goto('/');

      await layout.navigateToDesktopLink('Trending');
      await expect(page).toHaveURL('/search');

      await layout.navigateToDesktopLink('Categories');
      await expect(page).toHaveURL('/explore');

      await layout.navigateToDesktopLink('Saved');
      await expect(page).toHaveURL('/profile');

      await layout.navigateToDesktopLink('Home');
      await expect(page).toHaveURL('/');
    });

    test('should navigate to search on desktop search click', async ({ page }) => {
      await page.goto('/');
      await layout.clickDesktopSearch();
      await expect(page).toHaveURL('/search');
    });

    test('should navigate to profile on desktop profile click', async ({ page }) => {
      await page.goto('/');
      await layout.clickDesktopProfile();
      await expect(page).toHaveURL('/profile');
    });

    test('should highlight active desktop nav link', async ({ page }) => {
      await page.goto('/');
      expect(await layout.getActiveDesktopLink()).toBe('Home');

      await layout.navigateToDesktopLink('Categories');
      expect(await layout.getActiveDesktopLink()).toBe('Categories');
    });
  });

  test('should redirect unknown routes to home', async ({ page }) => {
    await page.goto('/nonexistent-page');
    await expect(page).toHaveURL('/');
  });
});
