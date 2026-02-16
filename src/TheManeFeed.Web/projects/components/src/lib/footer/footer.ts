import { Component } from '@angular/core';
import { LogoComponent } from '../logo/logo';

@Component({
  selector: 'lib-footer',
  imports: [LogoComponent],
  template: `
    <footer class="footer">
      <div class="footer-top">
        <div class="brand">
          <lib-logo variant="light" />
          <p class="description">
            Your daily dose of hair news, trends, and transformations.
            Curated for the style-obsessed.
          </p>
        </div>
        <div class="columns">
          <div class="column">
            <h4 class="column-title">Categories</h4>
            <a class="column-link">Color Trends</a>
            <a class="column-link">Haircuts</a>
            <a class="column-link">Styling</a>
            <a class="column-link">Celebrity</a>
          </div>
          <div class="column">
            <h4 class="column-title">Company</h4>
            <a class="column-link">About</a>
            <a class="column-link">Contact</a>
            <a class="column-link">Careers</a>
            <a class="column-link">Privacy</a>
          </div>
          <div class="column">
            <h4 class="column-title">Connect</h4>
            <a class="column-link">Instagram</a>
            <a class="column-link">TikTok</a>
            <a class="column-link">Pinterest</a>
            <a class="column-link">Newsletter</a>
          </div>
        </div>
      </div>
      <div class="footer-bottom">
        <span class="copyright">&copy; 2026 The Mane Feed. All rights reserved.</span>
        <span class="made-with">Made with &#10084; for hair lovers</span>
      </div>
    </footer>
  `,
  styleUrl: './footer.scss',
})
export class FooterComponent {}
