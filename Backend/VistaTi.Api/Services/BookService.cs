using Microsoft.EntityFrameworkCore;
using VistaTi.Api.Models;
using VistaTi.Api.Data;
namespace VistaTi.Api.Services;

public class BookService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;

    public BookService(AppDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    // Método para obtener favoritos
    public async Task<List<Favorite>> GetFavoritesAsync(int userId)
    {
        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    // Método para agregar un favorito
   public async Task<bool> AddFavoriteAsync(Favorite favorite)
{
    // VALIDACIÓN PARA EL TEST:
    // El test espera una ArgumentException si faltan campos obligatorios
    if (string.IsNullOrWhiteSpace(favorite.Title) || string.IsNullOrWhiteSpace(favorite.ExternalId))
    {
        throw new ArgumentException("El título y el ExternalId son obligatorios.");
    }

    // Verificar si ya existe por ExternalId para evitar duplicados
    var exists = await _context.Favorites
        .AnyAsync(f => f.ExternalId == favorite.ExternalId && f.UserId == favorite.UserId);

    if (exists) return false;

    _context.Favorites.Add(favorite);
    await _context.SaveChangesAsync();
    return true;
}
    // Método para eliminar un favorito
    public async Task<bool> DeleteFavoriteAsync(int id)
    {
        var favorite = await _context.Favorites.FindAsync(id);
        if (favorite == null) return false;

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
        return true;
    }

    // Método para buscar en la API externa (Open Library)
    public async Task<object> SearchBooksAsync(string query)
    {
        var response = await _httpClient.GetAsync($"https://openlibrary.org/search.json?q={query}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}