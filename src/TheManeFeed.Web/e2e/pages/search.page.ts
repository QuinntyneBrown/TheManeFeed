import { type Locator, type Page, expect } from '@playwright/test';

export class SearchPage {
  readonly page: Page;

  readonly pageTitle: Locator;
  readonly searchBar: Locator;
  readonly searchInput: Locator;

  // Before search
  readonly recentSection: Locator;
  readonly recentHeader: Locator;
  readonly clearRecentBtn: Locator;
  readonly recentItems: Locator;
  readonly trendingSearchSection: Locator;
  readonly trendingPills: Locator;
  readonly topStoriesSection: Locator;
  readonly topStoriesCards: Locator;

  // After search
  readonly resultsSection: Locator;
  readonly resultsCards: Locator;

  constructor(page: Page) {
    this.page = page;

    this.pageTitle = page.locator('.search-page .page-title');
    this.searchBar = page.locator('lib-search-bar');
    this.searchInput = this.searchBar.locator('input.search-input');

    this.recentSection = page.locator('.recent');
    this.recentHeader = this.recentSection.locator('.section-header');
    this.clearRecentBtn = this.recentSection.locator('.clear-btn');
    this.recentItems = page.locator('.recent-item');
    this.trendingSearchSection = page.locator('.trending-searches');
    this.trendingPills = this.trendingSearchSection.locator('lib-category-pill');
    this.topStoriesSection = page.locator('.top-stories');
    this.topStoriesCards = this.topStoriesSection.locator('lib-standard-card');

    this.resultsSection = page.locator('.results');
    this.resultsCards = this.resultsSection.locator('lib-standard-card');
  }

  async goto(): Promise<void> {
    await this.page.goto('/search');
  }

  async gotoWithQuery(query: string): Promise<void> {
    await this.page.goto(`/search?q=${encodeURIComponent(query)}`);
  }

  async expectLoaded(): Promise<void> {
    await expect(this.pageTitle).toBeVisible();
    await expect(this.pageTitle).toHaveText('Search');
  }

  async search(query: string): Promise<void> {
    await this.searchInput.fill(query);
    await this.searchInput.press('Enter');
  }

  async getRecentSearchTexts(): Promise<string[]> {
    const texts = await this.recentItems.locator('span').allTextContents();
    return texts.map((t) => t.trim());
  }

  async clickRecentSearch(index: number): Promise<void> {
    await this.recentItems.nth(index).click();
  }

  async clearRecentSearches(): Promise<void> {
    await this.clearRecentBtn.click();
  }

  async getTrendingPillLabels(): Promise<string[]> {
    const labels = await this.trendingPills.locator('button').allTextContents();
    return labels.map((l) => l.trim());
  }

  async clickTrendingPill(label: string): Promise<void> {
    await this.trendingPills.filter({ hasText: label }).locator('button').click();
  }

  async getTopStoriesCount(): Promise<number> {
    return this.topStoriesCards.count();
  }

  async getResultsCount(): Promise<number> {
    return this.resultsCards.count();
  }

  async expectResultsVisible(): Promise<void> {
    await expect(this.resultsSection).toBeVisible();
  }

  async expectDefaultViewVisible(): Promise<void> {
    await expect(this.recentSection).toBeVisible();
  }

  async clickResultCard(index: number): Promise<void> {
    await this.resultsCards.nth(index).click();
  }
}
