import { Component, input, output } from '@angular/core';

@Component({
  selector: 'lib-category-pill',
  template: `
    <button
      class="pill"
      [class.active]="active()"
      (click)="clicked.emit()"
    >
      {{ label() }}
    </button>
  `,
  styleUrl: './category-pill.scss',
})
export class CategoryPillComponent {
  readonly label = input.required<string>();
  readonly active = input(false);
  readonly clicked = output<void>();
}
