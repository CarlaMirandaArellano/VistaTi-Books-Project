using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Asegúrate de tener esta importación
using VistaTi.Api.DTOs;
using VistaTi.Api.Models;
using VistaTi.Api.Services;
using VistaTi.Api.Data;

namespace VistaTi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    // Cambiamos ApplicationDbContext por AppDbContext
    private readonly AppDbContext _context; 
    private readonly BookService _bookService;

    public BooksController(AppDbContext context, BookService bookService)
    {
        _context = context;
        _bookService = bookService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("El término de búsqueda no puede estar vacío.");
        try {
            var results = await _bookService.SearchBooksAsync(query);
            return Ok(results);
        } catch (Exception ex) {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavorites()
    {
        try {
            // Es mejor traerlos directamente del context para asegurar que ves lo de SQL
            var favorites = await _context.Favorites.Where(f => f.UserId == 1).ToListAsync();
            return Ok(favorites);
        } catch (Exception ex) {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpPost("favorites")]
    public async Task<IActionResult> AddFavorite([FromBody] BookDto bookDto)
    {
        if (bookDto == null) return BadRequest("Datos inválidos.");

        try 
        {
            // Verificamos si ya existe para evitar duplicados (Error 409)
            var exists = await _context.Favorites.AnyAsync(f => f.ExternalId == bookDto.ExternalId && f.UserId == 1);
            if (exists) return Conflict("El libro ya está en tus favoritos.");

            var favorite = new Favorite {
                ExternalId = bookDto.ExternalId,
                Title = bookDto.Title,
                Authors = bookDto.Authors != null ? string.Join(", ", bookDto.Authors) : "Desconocido",
                FirstPublishYear = bookDto.FirstPublishYear,
                CoverUrl = bookDto.CoverUrl,
                UserId = 1 
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync(); // Aquí se genera el ID real

            return Ok(favorite); 
        }
        catch (Exception ex) 
        {
            return StatusCode(500, $"Error interno al guardar: {ex.Message}");
        }
    }

    [HttpDelete("favorites/{id}")]
    public async Task<IActionResult> DeleteFavorite(int id)
    {
        try {
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null) return NotFound(new { message = "El libro no existe en favoritos." });

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Eliminado correctamente." });
        } catch (Exception ex) {
            return StatusCode(500, $"Error al eliminar: {ex.Message}");
        }
    }
}