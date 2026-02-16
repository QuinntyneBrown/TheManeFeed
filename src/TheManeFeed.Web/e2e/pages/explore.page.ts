import { type Locator, type Page, expect } from '@playwright/test';

export class ExplorePage {
  readonly page: Page;

  readonly pageTitle: Locator;
  readonly searchBar: Locator;
  readonly searchInput: Locator;
  readonly categoriesTitle: Locator;
  readonly categoryCards: Locator;
  readonly popularSection: Locator;
  readonly popularTitle: Locator;
  readonly popularCards: Locator;

  constructor(page: Page) {
    this.page = page;

    this.pageTitle = page.locator('.explore-page .page-title');
    this.searchBar = page.locator('lib-search-bar');
    this.searchInput = this.searchBar.locator('input.search-input');
    this.categoriesTitle = page.locator('.browse-categories .section-title');
    this.categoryCards = page.locator('.category-grid .category-card');
    this.popularSection = page.locator('.popular');
    this.popularTitle = this.popularSection.locator('.section-title');
    this.popularCards = this.popularSection.locator('lib-standard-card');
  }

  async goto(): Promise<void> {
    await this.page.goto('/explore');
  }

  async expectLoaded(): Promise<void> {
    await expect(this.pageTitle).toBeVisible();
    await expect(this.pageTitle).toHaveText('Explore');
  }

  async getCategoryNames(): Promise<string[]> {
    const names = await this.categoryCards.locator('.category-name').allTextContents();
    return names.map((n) => n.trim());
  }

  async getCategoryCount(): Promise<number> {
    return this.categoryCards.count();
  }

  async clickCategory(name: string): Promise<void> {
    await this.categoryCards.filter({ hasText: name }).click();
  }

  async search(query: string): Promise<void> {
    await this.searchInput.fill(query);
    await this.searchInput.press('Enter');
  }

  async getPopularCardCount(): Promise<number> {
    return this.popularCards.count();
  }

  async clickPopularCard(index: number): Promise<void> {
    await this.popularCards.nth(index).click();
  }
}
