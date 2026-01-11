import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SearchStateService {
  // Guardamos los libros y el término buscado
  private searchResultsSubject = new BehaviorSubject<any[]>([]);
  private lastQuerySubject = new BehaviorSubject<string>('');

  searchResults$ = this.searchResultsSubject.asObservable();
  lastQuery$ = this.lastQuerySubject.asObservable();

  saveResults(results: any[], query: string) {
    this.searchResultsSubject.next(results);
    this.lastQuerySubject.next(query);
  }


  getCurrentResults() { return this.searchResultsSubject.value; }
  getCurrentQuery() { return this.lastQuerySubject.value; }
}