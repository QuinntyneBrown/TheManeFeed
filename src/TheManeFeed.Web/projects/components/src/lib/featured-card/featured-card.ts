import { Component, input } from '@angular/core';

@Component({
  selector: 'lib-featured-card',
  template: `
    <article class="featured-card">
      <div class="image" [style.background-image]="'url(' + imageUrl() + ')'"></div>
      <div class="content">
        <span class="category">{{ category() }}</span>
        <h3 class="title">{{ title() }}</h3>
        <div class="meta">
          <span class="source">{{ source() }}</span>
          <span class="dot">&middot;</span>
          <span class="time">{{ timeAgo() }}</span>
        </div>
      </div>
    </article>
  `,
  styleUrl: './featured-card.scss',
})
export class FeaturedCardComponent {
  readonly imageUrl = input.required<string>();
  readonly category = input.required<string>();
  readonly title = input.required<string>();
  readonly source = input.required<string>();
  readonly timeAgo = input.required<string>();
}
