export interface Book {
    id: number;            // ID de la base de datos (para eliminar favoritos)
    externalId: string;     // ID único de Open Library (ej: /works/OL123W)
    title: string;
    authors: string[];      // Lista de autores
    firstPublishYear?: string;
    coverUrl?: string;
}
