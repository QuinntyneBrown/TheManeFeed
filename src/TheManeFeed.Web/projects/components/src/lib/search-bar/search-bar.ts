import { Component, input, output } from '@angular/core';
import { IconComponent } from '../icon/icon';

@Component({
  selector: 'lib-search-bar',
  imports: [IconComponent],
  template: `
    <div class="search-bar">
      <lib-icon name="search" [size]="18" />
      <input
        class="search-input"
        type="text"
        [placeholder]="placeholder()"
        (input)="onInput($event)"
        (keyup.enter)="onSearch()"
      />
    </div>
  `,
  styleUrl: './search-bar.scss',
})
export class SearchBarComponent {
  readonly placeholder = input('Search hair trends, styles, news...');
  readonly search = output<string>();

  private currentValue = '';

  protected onInput(event: Event): void {
    this.currentValue = (event.target as HTMLInputElement).value;
  }

  protected onSearch(): void {
    this.search.emit(this.currentValue);
  }
}
