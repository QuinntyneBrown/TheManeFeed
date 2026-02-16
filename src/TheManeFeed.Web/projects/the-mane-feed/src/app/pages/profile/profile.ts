import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { CategoryPillComponent, IconComponent } from 'components';

interface SettingsItem {
  label: string;
  icon: string;
}

@Component({
  selector: 'app-profile',
  imports: [CategoryPillComponent, IconComponent],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class ProfileComponent {
  protected readonly displayName = signal('Amara Johnson');
  protected readonly username = signal('@amaraj');
  protected readonly avatarUrl = signal<string | null>(null);

  protected readonly stats = signal({
    saved: 24,
    topics: 6,
    collections: 3,
  });

  protected readonly interests = signal([
    { id: 1, name: 'Color Trends', slug: 'color', active: true },
    { id: 3, name: 'Haircuts & Styles', slug: 'cuts', active: true },
    { id: 4, name: 'Hair Care', slug: 'care', active: true },
    { id: 7, name: 'Styling', slug: 'styling', active: true },
    { id: 5, name: 'Products & Reviews', slug: 'products', active: false },
    { id: 2, name: 'Celebrity Hair', slug: 'celebrity', active: false },
  ]);

  protected readonly settingsItems: SettingsItem[] = [
    { label: 'Notifications', icon: 'bell' },
    { label: 'Appearance', icon: 'heart' },
    { label: 'Privacy & Data', icon: 'bookmark' },
    { label: 'Help & Support', icon: 'search' },
  ];

  constructor(private readonly router: Router) {}

  protected onInterestToggle(slug: string): void {
    this.interests.update((items) =>
      items.map((item) =>
        item.slug === slug ? { ...item, active: !item.active } : item
      )
    );
  }

  protected onSignOut(): void {
    this.router.navigateByUrl('/');
  }
}
