import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Book } from '../models/book.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  private apiUrl = 'http://localhost:5272/api/books';

  //  Estado reactivo
  private favoritesSubject = new BehaviorSubject<Book[]>([]);
  favorites$ = this.favoritesSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadFavorites();
  }

  //  Carga inicial
  private loadFavorites(): void {
    this.http.get<Book[]>(`${this.apiUrl}/favorites`)
      .subscribe(data => {
        this.favoritesSubject.next([...data]); // 👈 nueva referencia
      });
  }

  // REQUERIMIENTO 1: Buscar libros
  searchBooks(query: string): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.apiUrl}/search?query=${query}`);
  }

  // REQUERIMIENTO 2: Obtener favoritos (reactivo)
  getFavorites(): Observable<Book[]> {
    return this.favorites$;
  }

  // REQUERIMIENTO 2: Agregar favorito
  addFavorite(book: Book): Observable<any> {
    return this.http.post<Book>(`${this.apiUrl}/favorites`, book).pipe(
      tap((createdBook) => {
        const current = this.favoritesSubject.value;
        this.favoritesSubject.next([...current, createdBook]);
      })
    );
  }

  // REQUERIMIENTO 2: Eliminar favorito
  deleteFavorite(id: number): Observable<void> {
  return this.http.delete<void>(`${this.apiUrl}/favorites/${id}`).pipe(
    tap(() => {
      const current = this.favoritesSubject.value;
      this.favoritesSubject.next(
        current.filter(fav => fav.id !== id)
      );
    })
  );
}


};



