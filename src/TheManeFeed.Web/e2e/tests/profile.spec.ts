import { test, expect } from '@playwright/test';
import { ProfilePage } from '../pages/profile.page';
import { mockAllApis } from '../helpers/api-mocks';

test.describe('Profile Page', () => {
  let profile: ProfilePage;

  test.beforeEach(async ({ page }) => {
    await mockAllApis(page);
    profile = new ProfilePage(page);
    await profile.goto();
  });

  test.describe('Profile Header', () => {
    test('should display the user display name', async () => {
      await profile.expectLoaded();
      const name = await profile.getDisplayName();
      expect(name).toBe('Amara Johnson');
    });

    test('should display the username', async () => {
      await profile.expectLoaded();
      const username = await profile.getUsername();
      expect(username).toBe('@amaraj');
    });

    test('should display avatar initial when no avatar image', async () => {
      await profile.expectLoaded();
      const initial = await profile.getAvatarInitial();
      expect(initial).toBe('A');
    });
  });

  test.describe('Stats', () => {
    test('should display saved count', async () => {
      await profile.expectLoaded();
      const value = await profile.getStatValue(profile.savedStat);
      expect(value).toBe('24');
      const label = await profile.getStatLabel(profile.savedStat);
      expect(label).toBe('Saved');
    });

    test('should display topics count', async () => {
      await profile.expectLoaded();
      const value = await profile.getStatValue(profile.topicsStat);
      expect(value).toBe('6');
      const label = await profile.getStatLabel(profile.topicsStat);
      expect(label).toBe('Topics');
    });

    test('should display collections count', async () => {
      await profile.expectLoaded();
      const value = await profile.getStatValue(profile.collectionsStat);
      expect(value).toBe('3');
      const label = await profile.getStatLabel(profile.collectionsStat);
      expect(label).toBe('Collections');
    });
  });

  test.describe('Your Interests', () => {
    test('should display interest pills', async () => {
      await profile.expectLoaded();
      const labels = await profile.getInterestLabels();
      expect(labels.length).toBeGreaterThan(0);
      expect(labels).toContain('Color Trends');
      expect(labels).toContain('Haircuts & Styles');
    });

    test('should have some interests active by default', async () => {
      await profile.expectLoaded();
      const activeLabels = await profile.getActiveInterestLabels();
      expect(activeLabels.length).toBeGreaterThan(0);
    });

    test('should toggle interest active state on click', async () => {
      await profile.expectLoaded();

      const beforeLabels = await profile.getActiveInterestLabels();
      const targetLabel = 'Products & Reviews';

      // Click an inactive interest to activate it
      await profile.toggleInterest(targetLabel);
      const afterLabels = await profile.getActiveInterestLabels();

      if (beforeLabels.includes(targetLabel)) {
        expect(afterLabels).not.toContain(targetLabel);
      } else {
        expect(afterLabels).toContain(targetLabel);
      }
    });
  });

  test.describe('Settings', () => {
    test('should display all settings items', async () => {
      await profile.expectLoaded();
      const labels = await profile.getSettingsLabels();
      expect(labels).toEqual([
        'Notifications',
        'Appearance',
        'Privacy & Data',
        'Help & Support',
      ]);
    });

    test('should display chevron icon on each settings item', async ({ page }) => {
      await profile.expectLoaded();
      const icons = profile.settingsItems.locator('lib-icon');
      const count = await icons.count();
      expect(count).toBe(4);
    });
  });

  test.describe('Sign Out', () => {
    test('should display sign out button', async () => {
      await profile.expectLoaded();
      await expect(profile.signOutBtn).toBeVisible();
      await expect(profile.signOutBtn).toHaveText('Sign Out');
    });

    test('should navigate to home on sign out', async ({ page }) => {
      await profile.expectLoaded();
      await profile.clickSignOut();
      await expect(page).toHaveURL('/');
    });
  });
});
