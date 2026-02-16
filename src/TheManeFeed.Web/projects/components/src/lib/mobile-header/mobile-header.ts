import { Component, output } from '@angular/core';
import { LogoComponent } from '../logo/logo';
import { IconComponent } from '../icon/icon';

@Component({
  selector: 'lib-mobile-header',
  imports: [LogoComponent, IconComponent],
  template: `
    <header class="mobile-header">
      <lib-logo />
      <div class="actions">
        <button class="icon-btn" (click)="searchClick.emit()">
          <lib-icon name="search" [size]="22" />
        </button>
        <button class="icon-btn" (click)="notificationClick.emit()">
          <lib-icon name="bell" [size]="22" />
        </button>
      </div>
    </header>
  `,
  styleUrl: './mobile-header.scss',
})
export class MobileHeaderComponent {
  readonly searchClick = output<void>();
  readonly notificationClick = output<void>();
}
