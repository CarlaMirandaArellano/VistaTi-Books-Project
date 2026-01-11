import { Routes } from '@angular/router';
import { BookSearchComponent } from './components/book-search/book-search.component';
import { FavoritesComponent } from './components/Favorites/favorites.component';

export const routes: Routes = [
  { path: '', redirectTo: 'search', pathMatch: 'full' },
  { path: 'search', component: BookSearchComponent },
  { path: 'favorites', component: FavoritesComponent },
  { path: '**', redirectTo: 'search' }
];