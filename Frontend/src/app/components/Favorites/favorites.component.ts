import { Component } from '@angular/core';
import { BookService } from '../../services/book.service';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Book } from '../../models/book.model';


@Component({
  selector: 'app-favorites',
  standalone: true,
  imports: [CommonModule], // <--- ESTO ACTIVA *ngIf y *ngFor
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.css']
})
export class FavoritesComponent {

  favorites$: Observable<Book[]>;

  constructor(private bookService: BookService) {
    this.favorites$ = this.bookService.getFavorites();
  }

removeFavorite(id: number) {
  this.bookService.deleteFavorite(id).subscribe();
}




}
