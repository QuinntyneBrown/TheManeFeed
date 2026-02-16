import { type Locator, type Page, expect } from '@playwright/test';

export class AppLayoutPage {
  readonly page: Page;

  // Mobile
  readonly mobileHeader: Locator;
  readonly mobileSearchBtn: Locator;
  readonly mobileNotificationBtn: Locator;
  readonly mobileBottomNav: Locator;

  // Desktop
  readonly desktopHeader: Locator;
  readonly desktopNavLinks: Locator;
  readonly desktopSearchBtn: Locator;
  readonly desktopProfileBtn: Locator;

  // Shared
  readonly routerOutlet: Locator;

  constructor(page: Page) {
    this.page = page;

    const mobileSection = page.locator('.mobile-only');
    this.mobileHeader = mobileSection.locator('lib-mobile-header');
    this.mobileSearchBtn = this.mobileHeader.locator('button.icon-btn').first();
    this.mobileNotificationBtn = this.mobileHeader.locator('button.icon-btn').nth(1);
    this.mobileBottomNav = mobileSection.locator('lib-mobile-bottom-nav');

    const desktopSection = page.locator('.desktop-only');
    this.desktopHeader = desktopSection.locator('lib-desktop-header');
    this.desktopNavLinks = this.desktopHeader.locator('.nav-link');
    this.desktopSearchBtn = this.desktopHeader.locator('button.icon-btn');
    this.desktopProfileBtn = this.desktopHeader.locator('.profile-btn');

    this.routerOutlet = page.locator('router-outlet');
  }

  async navigateToTab(tabName: string): Promise<void> {
    const tab = this.mobileBottomNav.locator(`.nav-item`).filter({ hasText: tabName });
    await tab.click();
  }

  async getActiveTab(): Promise<string> {
    const activeItem = this.mobileBottomNav.locator('.nav-item.active');
    return (await activeItem.textContent())?.trim() ?? '';
  }

  async navigateToDesktopLink(linkText: string): Promise<void> {
    await this.desktopNavLinks.filter({ hasText: linkText }).click();
  }

  async getActiveDesktopLink(): Promise<string> {
    const activeLink = this.desktopHeader.locator('.nav-link.active');
    return (await activeLink.textContent())?.trim() ?? '';
  }

  async expectMobileHeaderVisible(): Promise<void> {
    await expect(this.mobileHeader).toBeVisible();
  }

  async expectBottomNavVisible(): Promise<void> {
    await expect(this.mobileBottomNav).toBeVisible();
  }

  async clickMobileSearch(): Promise<void> {
    await this.mobileSearchBtn.click();
  }

  async clickMobileNotification(): Promise<void> {
    await this.mobileNotificationBtn.click();
  }

  async clickDesktopSearch(): Promise<void> {
    await this.desktopSearchBtn.click();
  }

  async clickDesktopProfile(): Promise<void> {
    await this.desktopProfileBtn.click();
  }
}
