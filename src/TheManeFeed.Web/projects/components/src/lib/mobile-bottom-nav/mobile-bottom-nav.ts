import { Component, input, output } from '@angular/core';
import { IconComponent } from '../icon/icon';

@Component({
  selector: 'lib-mobile-bottom-nav',
  imports: [IconComponent],
  template: `
    <nav class="bottom-nav">
      @for (item of navItems; track item.id) {
        <button
          class="nav-item"
          [class.active]="activeTab() === item.id"
          (click)="tabChange.emit(item.id)"
        >
          <lib-icon [name]="item.icon" [size]="22" />
          <span class="nav-label">{{ item.label }}</span>
        </button>
      }
    </nav>
  `,
  styleUrl: './mobile-bottom-nav.scss',
})
export class MobileBottomNavComponent {
  readonly activeTab = input('home');
  readonly tabChange = output<string>();

  protected readonly navItems = [
    { id: 'home', icon: 'house', label: 'Home' },
    { id: 'explore', icon: 'compass', label: 'Explore' },
    { id: 'trending', icon: 'flame', label: 'Trending' },
    { id: 'saved', icon: 'bookmark', label: 'Saved' },
  ];
}
