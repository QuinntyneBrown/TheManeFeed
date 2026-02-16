import { Component, input, output } from '@angular/core';
import { LogoComponent } from '../logo/logo';
import { IconComponent } from '../icon/icon';

@Component({
  selector: 'lib-desktop-header',
  imports: [LogoComponent, IconComponent],
  template: `
    <header class="desktop-header">
      <lib-logo />
      <nav class="nav-links">
        @for (link of links; track link.id) {
          <a
            class="nav-link"
            [class.active]="activeLink() === link.id"
            (click)="linkClick.emit(link.id)"
          >
            {{ link.label }}
          </a>
        }
      </nav>
      <div class="actions">
        <button class="icon-btn" (click)="searchClick.emit()">
          <lib-icon name="search" [size]="20" />
        </button>
        <button class="profile-btn" (click)="profileClick.emit()">
          <div class="profile-avatar"></div>
        </button>
      </div>
    </header>
  `,
  styleUrl: './desktop-header.scss',
})
export class DesktopHeaderComponent {
  readonly activeLink = input('home');
  readonly linkClick = output<string>();
  readonly searchClick = output<void>();
  readonly profileClick = output<void>();

  protected readonly links = [
    { id: 'home', label: 'Home' },
    { id: 'trending', label: 'Trending' },
    { id: 'categories', label: 'Categories' },
    { id: 'saved', label: 'Saved' },
  ];
}
