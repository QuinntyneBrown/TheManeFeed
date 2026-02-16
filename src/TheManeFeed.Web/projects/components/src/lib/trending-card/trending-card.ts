import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'lib-trending-card',
  template: `
    <article class="trending-card">
      <span class="rank">{{ rankFormatted() }}</span>
      <div class="content">
        <h4 class="title">{{ title() }}</h4>
        <span class="meta">{{ meta() }}</span>
      </div>
    </article>
  `,
  styleUrl: './trending-card.scss',
})
export class TrendingCardComponent {
  readonly rank = input.required<number>();
  readonly title = input.required<string>();
  readonly meta = input.required<string>();

  protected readonly rankFormatted = computed(() =>
    String(this.rank()).padStart(2, '0')
  );
}
