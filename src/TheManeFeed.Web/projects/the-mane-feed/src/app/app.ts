import { Component, computed, signal } from '@angular/core';
import { Router, RouterOutlet, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs';
import {
  MobileHeaderComponent,
  MobileBottomNavComponent,
  DesktopHeaderComponent,
  FooterComponent,
} from 'components';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MobileHeaderComponent,
    MobileBottomNavComponent,
    DesktopHeaderComponent,
    FooterComponent,
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  private readonly activeRoute = signal('home');

  protected readonly activeTab = computed(() => {
    const route = this.activeRoute();
    if (route === '' || route === 'home') return 'home';
    if (route.startsWith('explore')) return 'explore';
    if (route.startsWith('search') || route.startsWith('trending')) return 'trending';
    if (route.startsWith('profile') || route.startsWith('saved')) return 'saved';
    return 'home';
  });

  protected readonly activeLink = computed(() => {
    const route = this.activeRoute();
    if (route === '' || route === 'home') return 'home';
    if (route.startsWith('trending')) return 'trending';
    if (route.startsWith('explore')) return 'categories';
    if (route.startsWith('saved') || route.startsWith('profile')) return 'saved';
    return 'home';
  });

  constructor(private readonly router: Router) {
    this.router.events
      .pipe(filter((e): e is NavigationEnd => e instanceof NavigationEnd))
      .subscribe((e) => {
        const path = e.urlAfterRedirects.split('/').filter(Boolean)[0] ?? '';
        this.activeRoute.set(path);
      });
  }

  protected onTabChange(tab: string): void {
    const routeMap: Record<string, string> = {
      home: '/',
      explore: '/explore',
      trending: '/search',
      saved: '/profile',
    };
    this.router.navigateByUrl(routeMap[tab] ?? '/');
  }

  protected onLinkClick(link: string): void {
    const routeMap: Record<string, string> = {
      home: '/',
      trending: '/search',
      categories: '/explore',
      saved: '/profile',
    };
    this.router.navigateByUrl(routeMap[link] ?? '/');
  }

  protected onSearchClick(): void {
    this.router.navigateByUrl('/search');
  }

  protected onProfileClick(): void {
    this.router.navigateByUrl('/profile');
  }
}
