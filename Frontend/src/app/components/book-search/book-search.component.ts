import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book.model';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-book-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-search.component.html',
  styleUrl: './book-search.component.css'
})
export class BookSearchComponent {
  query: string = '';
  books: any[] = []; // Usamos any temporalmente para procesar la respuesta de la API
  loading: boolean = false;

  constructor(private bookService: BookService,private cdr: ChangeDetectorRef ) {}

  search() {
    if (!this.query.trim()) return;
    
    this.loading = true;
    this.books = []; // Limpiamos la lista anterior para que el usuario vea que algo pasa

    this.bookService.searchBooks(this.query).subscribe({
      next: (response: any) => {
        // CORRECCIÓN CLAVE: Open Library devuelve un objeto, no un array directo.
        // Debemos buscar la propiedad 'docs'
        const rawData = typeof response === 'string' ? JSON.parse(response) : response;
        
        if (rawData && rawData.docs) {
          // Mapeamos para que las portadas funcionen de una vez
          this.books = rawData.docs.map((b: any) => ({
            title: b.title,
            coverUrl: b.cover_i ? `https://covers.openlibrary.org/b/id/${b.cover_i}-M.jpg` : null,
            firstPublishYear: b.first_publish_year,
            externalId: b.key?.replace('/works/', '') || ''
          }));
        }

        this.loading = false;
        this.cdr.detectChanges(); // 3. FORZAMOS el refresco de la pantalla
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
    externalId: book.externalId || book.key,
    coverUrl: book.coverUrl || '',
    userId: 1
  };

  // El casteo 'as any' silencia el error TS2345 de tu captura
  this.bookService.addFavorite(favoritePayload as any).subscribe({
    next: (res: any) => {
      // CLAVE: Asignamos el ID que viene de SQL al objeto local
      book.id = res.id || res.Id; 
      alert('¡Agregado a favoritos!');
    },
    error: (err) => console.error('Bad Request 400:', err.error)
  });
  }
}