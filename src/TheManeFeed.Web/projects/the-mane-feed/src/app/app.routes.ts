import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home').then(m => m.HomeComponent),
  },
  {
    path: 'article/:id',
    loadComponent: () => import('./pages/article-detail/article-detail').then(m => m.ArticleDetailComponent),
  },
  {
    path: 'explore',
    loadComponent: () => import('./pages/explore/explore').then(m => m.ExploreComponent),
  },
  {
    path: 'search',
    loadComponent: () => import('./pages/search/search').then(m => m.SearchComponent),
  },
  {
    path: 'profile',
    loadComponent: () => import('./pages/profile/profile').then(m => m.ProfileComponent),
  },
  {
    path: '**',
    redirectTo: '',
  },
];
