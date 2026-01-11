import { Component, OnInit } from '@angular/core';
import { BookService } from '../../services/book.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-favorites',
  standalone: true, 
  imports: [CommonModule], // <--- ESTO ACTIVA *ngIf y *ngFor
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.css']
})
export class FavoritesComponent implements OnInit {
  favorites: any[] = [];

  constructor(private bookService: BookService) {}

  ngOnInit(): void {
    this.loadFavorites();
  }

  loadFavorites(): void {
    this.bookService.getFavorites().subscribe({
      next: (data) => {
        this.favorites = data;
        console.log('Datos cargados de SQL:', this.favorites);
      },
      error: (err) => console.error('Error al cargar:', err)
    });
  }

  removeFavorite(id: any) {
    const idABorrar = Number(id);
    if (!idABorrar) return;
  
    console.log('Solicitando borrar ID:', idABorrar);
  
    this.bookService.deleteFavorite(idABorrar).subscribe({
      next: () => {
        console.log('Borrado exitoso en SQL');
        // USAMOS UNA NUEVA REFERENCIA: Esto obliga a Angular a refrescar la pantalla
        this.favorites = [...this.favorites.filter(fav => {
          const currentId = fav.id ?? fav.Id; 
          return Number(currentId) !== idABorrar;
        })];
      },
      error: (err) => {
        // Si el servidor da 404, significa que ya no existe en DB, 
        // así que igual lo quitamos de la vista para limpiar la pantalla.
        if (err.status === 404) {
          this.favorites = [...this.favorites.filter(fav => (fav.id ?? fav.Id) !== idABorrar)];
        }
      }
    });
  }
}