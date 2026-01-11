import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookService } from '../../services/book.service';
import { ChangeDetectorRef } from '@angular/core';
import { SearchStateService } from '../../services/search-state.service'; //  nuevo servicio de búsqueda

@Component({
  selector: 'app-book-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-search.component.html',
  styleUrl: './book-search.component.css'
})
export class BookSearchComponent implements OnInit {
  query: string = '';
  books: any[] = [];
  loading: boolean = false;

  constructor(
    private bookService: BookService,
    private searchState: SearchStateService, 
    private cdr: ChangeDetectorRef 
  ) {}

  ngOnInit() {
    // 5. Al volver de Favoritos, recuperamos lo que había
    this.books = this.searchState.getCurrentResults();
    this.query = this.searchState.getCurrentQuery();
  }

  search() {
    if (!this.query.trim()) return;

    this.loading = true;
    this.books = []; 

    this.bookService.searchBooks(this.query).subscribe({
      next: (response: any) => {
        const rawData = typeof response === 'string' ? JSON.parse(response) : response;

        if (rawData && rawData.docs) {
          const mappedBooks = rawData.docs.map((b: any) => ({
            title: b.title,
            coverUrl: b.cover_i ? `https://covers.openlibrary.org/b/id/${b.cover_i}-M.jpg` : null,
            firstPublishYear: b.first_publish_year,
            externalId: b.key?.replace('/works/', '') || ''
          }));

          this.books = mappedBooks;
          // 6. GUARDAMOS los resultados en el servicio de estado
          this.searchState.saveResults(this.books, this.query);
        }

        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error:', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  addToFavorites(book: any) {
    const favoritePayload = {
      title: book.title,
      externalId: book.externalId,
      coverUrl: book.coverUrl,
      userId: 1
    };

    this.bookService.addFavorite(favoritePayload as any).subscribe({
      next: (res: any) => {
        alert('¡Agregado a favoritos!');
      },
      error: (err) => {
        if (err.status === 400 || err.status === 409) {
          alert(' No No No, ya lo tienes en favoritos');
        } else {
          alert('Ocurrió un error inesperado al guardar.');
          console.error(err);
        }
      }
    });
  }
}