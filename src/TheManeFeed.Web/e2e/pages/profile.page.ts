import { type Locator, type Page, expect } from '@playwright/test';

export class ProfilePage {
  readonly page: Page;

  readonly avatar: Locator;
  readonly avatarInitial: Locator;
  readonly displayName: Locator;
  readonly username: Locator;

  readonly statsRow: Locator;
  readonly savedStat: Locator;
  readonly topicsStat: Locator;
  readonly collectionsStat: Locator;

  readonly interestsSection: Locator;
  readonly interestPills: Locator;

  readonly settingsItems: Locator;
  readonly signOutBtn: Locator;

  constructor(page: Page) {
    this.page = page;

    this.avatar = page.locator('.avatar');
    this.avatarInitial = page.locator('.avatar-initial');
    this.displayName = page.locator('.display-name');
    this.username = page.locator('.username');

    this.statsRow = page.locator('.stats-row');
    this.savedStat = this.statsRow.locator('.stat').nth(0);
    this.topicsStat = this.statsRow.locator('.stat').nth(1);
    this.collectionsStat = this.statsRow.locator('.stat').nth(2);

    this.interestsSection = page.locator('.interests');
    this.interestPills = this.interestsSection.locator('lib-category-pill');

    this.settingsItems = page.locator('.settings-item');
    this.signOutBtn = page.locator('.sign-out-btn');
  }

  async goto(): Promise<void> {
    await this.page.goto('/profile');
  }

  async expectLoaded(): Promise<void> {
    await expect(this.displayName).toBeVisible();
  }

  async getDisplayName(): Promise<string> {
    return (await this.displayName.textContent())?.trim() ?? '';
  }

  async getUsername(): Promise<string> {
    return (await this.username.textContent())?.trim() ?? '';
  }

  async getAvatarInitial(): Promise<string> {
    return (await this.avatarInitial.textContent())?.trim() ?? '';
  }

  async getStatValue(statLocator: Locator): Promise<string> {
    return (await statLocator.locator('.stat-value').textContent())?.trim() ?? '';
  }

  async getStatLabel(statLocator: Locator): Promise<string> {
    return (await statLocator.locator('.stat-label').textContent())?.trim() ?? '';
  }

  async getInterestLabels(): Promise<string[]> {
    const labels = await this.interestPills.locator('button').allTextContents();
    return labels.map((l) => l.trim());
  }

  async getActiveInterestLabels(): Promise<string[]> {
    const labels = await this.interestPills.locator('button.active').allTextContents();
    return labels.map((l) => l.trim());
  }

  async toggleInterest(label: string): Promise<void> {
    await this.interestPills.filter({ hasText: label }).locator('button').click();
  }

  async getSettingsLabels(): Promise<string[]> {
    const labels = await this.settingsItems.locator('.settings-label').allTextContents();
    return labels.map((l) => l.trim());
  }

  async clickSignOut(): Promise<void> {
    await this.signOutBtn.click();
  }
}
