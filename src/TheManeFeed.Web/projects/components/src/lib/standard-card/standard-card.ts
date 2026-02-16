import { Component, input } from '@angular/core';

@Component({
  selector: 'lib-standard-card',
  template: `
    <article class="standard-card">
      <div
        class="thumbnail"
        [style.background-image]="'url(' + imageUrl() + ')'"
      ></div>
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
  styleUrl: './standard-card.scss',
})
export class StandardCardComponent {
  readonly imageUrl = input.required<string>();
  readonly category = input.required<string>();
  readonly title = input.required<string>();
  readonly source = input.required<string>();
  readonly timeAgo = input.required<string>();
}
