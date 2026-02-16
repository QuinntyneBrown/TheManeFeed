import { type Locator, type Page, expect } from '@playwright/test';

export class HomePage {
  readonly page: Page;

  readonly categoryPills: Locator;
  readonly featuredCard: Locator;
  readonly latestSection: Locator;
  readonly latestTitle: Locator;
  readonly latestCards: Locator;
  readonly trendingSection: Locator;
  readonly trendingTitle: Locator;
  readonly trendingCards: Locator;

  constructor(page: Page) {
    this.page = page;

    this.categoryPills = page.locator('.category-scroll lib-category-pill');
    this.featuredCard = page.locator('.featured lib-featured-card');
    this.latestSection = page.locator('.latest');
    this.latestTitle = this.latestSection.locator('.section-title');
    this.latestCards = this.latestSection.locator('lib-standard-card');
    this.trendingSection = page.locator('.trending');
    this.trendingTitle = this.trendingSection.locator('.section-title');
    this.trendingCards = this.trendingSection.locator('lib-trending-card');
  }

  async goto(): Promise<void> {
    await this.page.goto('/');
  }

  async expectLoaded(): Promise<void> {
    await expect(this.featuredCard).toBeVisible();
  }

  async getCategoryLabels(): Promise<string[]> {
    return this.categoryPills.locator('button').allTextContents();
  }

  async clickCategory(label: string): Promise<void> {
    await this.categoryPills.filter({ hasText: label }).locator('button').click();
  }

  async getActiveCategoryLabel(): Promise<string | null> {
    const active = this.categoryPills.locator('button.active');
    const count = await active.count();
    if (count === 0) return null;
    return (await active.first().textContent())?.trim() ?? null;
  }

  async getFeaturedTitle(): Promise<string> {
    return (await this.featuredCard.locator('.title').textContent())?.trim() ?? '';
  }

  async clickFeaturedCard(): Promise<void> {
    await this.featuredCard.click();
  }

  async getLatestCardTitles(): Promise<string[]> {
    const titles = await this.latestCards.locator('.title').allTextContents();
    return titles.map((t) => t.trim());
  }

  async clickLatestCard(index: number): Promise<void> {
    await this.latestCards.nth(index).click();
  }

  async getTrendingCardTitles(): Promise<string[]> {
    const titles = await this.trendingCards.locator('.title').allTextContents();
    return titles.map((t) => t.trim());
  }

  async getTrendingRanks(): Promise<string[]> {
    const ranks = await this.trendingCards.locator('.rank').allTextContents();
    return ranks.map((r) => r.trim());
  }

  async clickTrendingCard(index: number): Promise<void> {
    await this.trendingCards.nth(index).click();
  }
}
