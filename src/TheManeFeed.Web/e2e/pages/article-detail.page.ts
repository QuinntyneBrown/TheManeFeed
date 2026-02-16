import { type Locator, type Page, expect } from '@playwright/test';

export class ArticleDetailPage {
  readonly page: Page;

  readonly backButton: Locator;
  readonly shareButton: Locator;
  readonly bookmarkButton: Locator;
  readonly heroImage: Locator;
  readonly categoryLabel: Locator;
  readonly articleTitle: Locator;
  readonly authorAvatar: Locator;
  readonly authorName: Locator;
  readonly articleDate: Locator;
  readonly bodyContent: Locator;
  readonly bodySummary: Locator;
  readonly relatedSection: Locator;
  readonly relatedTitle: Locator;
  readonly relatedCards: Locator;

  constructor(page: Page) {
    this.page = page;

    this.backButton = page.locator('.top-bar .icon-btn').first();
    this.shareButton = page.locator('.top-actions .icon-btn').first();
    this.bookmarkButton = page.locator('.top-actions .icon-btn').nth(1);
    this.heroImage = page.locator('.hero-image');
    this.categoryLabel = page.locator('.category-label');
    this.articleTitle = page.locator('.article-title');
    this.authorAvatar = page.locator('.author-avatar');
    this.authorName = page.locator('.author-name');
    this.articleDate = page.locator('.article-date');
    this.bodyContent = page.locator('.body-content');
    this.bodySummary = page.locator('.body-text');
    this.relatedSection = page.locator('.related');
    this.relatedTitle = this.relatedSection.locator('.section-title');
    this.relatedCards = this.relatedSection.locator('lib-standard-card');
  }

  async goto(id: number): Promise<void> {
    await this.page.goto(`/article/${id}`);
  }

  async expectLoaded(): Promise<void> {
    await expect(this.articleTitle).toBeVisible();
  }

  async getTitle(): Promise<string> {
    return (await this.articleTitle.textContent())?.trim() ?? '';
  }

  async getCategory(): Promise<string> {
    return (await this.categoryLabel.textContent())?.trim() ?? '';
  }

  async getAuthorName(): Promise<string> {
    return (await this.authorName.textContent())?.trim() ?? '';
  }

  async getDate(): Promise<string> {
    return (await this.articleDate.textContent())?.trim() ?? '';
  }

  async hasHeroImage(): Promise<boolean> {
    return this.heroImage.isVisible();
  }

  async hasBody(): Promise<boolean> {
    const contentVisible = await this.bodyContent.isVisible().catch(() => false);
    const summaryVisible = await this.bodySummary.isVisible().catch(() => false);
    return contentVisible || summaryVisible;
  }

  async getBodyText(): Promise<string> {
    const contentVisible = await this.bodyContent.isVisible().catch(() => false);
    if (contentVisible) {
      return (await this.bodyContent.textContent())?.trim() ?? '';
    }
    return (await this.bodySummary.textContent())?.trim() ?? '';
  }

  async clickBack(): Promise<void> {
    await this.backButton.click();
  }

  async clickShare(): Promise<void> {
    await this.shareButton.click();
  }

  async clickBookmark(): Promise<void> {
    await this.bookmarkButton.click();
  }

  async getRelatedCardCount(): Promise<number> {
    return this.relatedCards.count();
  }

  async clickRelatedCard(index: number): Promise<void> {
    await this.relatedCards.nth(index).click();
  }
}
