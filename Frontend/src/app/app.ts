import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink], // Importante agregar estos dos
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent {
  title = 'VistaTi-Front';
}