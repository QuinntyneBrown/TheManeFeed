import { Component, input } from '@angular/core';

@Component({
  selector: 'lib-logo',
  template: `
    <div class="logo" [class.light]="variant() === 'light'">
      <div class="logo-mark">
        <span class="logo-letter">M</span>
      </div>
      <div class="logo-text">
        <span class="logo-title">The Mane Feed</span>
        <span class="logo-tagline">hair news &amp; trends</span>
      </div>
    </div>
  `,
  styleUrl: './logo.scss',
})
export class LogoComponent {
  readonly variant = input<'default' | 'light'>('default');
}
