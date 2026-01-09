using Microsoft.AspNetCore.Mvc;
using VistaTi.Api.DTOs;
using VistaTi.Api.Models;
using VistaTi.Api.Services;

namespace VistaTi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
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
            var favorites = await _bookService.GetFavoritesAsync(1);
            return Ok(favorites);
        } catch (Exception ex) {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpPost("favorites")]
    public async Task<IActionResult> AddFavorite([FromBody] BookDto bookDto)
    {
        if (bookDto == null) return BadRequest("Datos inválidos.");

        var favorite = new Favorite {
            ExternalId = bookDto.ExternalId,
            Title = bookDto.Title,
            Authors = bookDto.Authors != null ? string.Join(", ", bookDto.Authors) : "Desconocido",
            FirstPublishYear = bookDto.FirstPublishYear,
            CoverUrl = bookDto.CoverUrl,
            UserId = 1 
        };

        var success = await _bookService.AddFavoriteAsync(favorite);
        if (!success) return Conflict("Ya es favorito.");

        // CAMBIO CRÍTICO: Devolvemos el objeto 'favorite' que ya tiene el ID de SQL
        return Ok(favorite); 
    }

    [HttpDelete("favorites/{id}")]
    public async Task<IActionResult> DeleteFavorite(int id)
    {
        var success = await _bookService.DeleteFavoriteAsync(id);
        if (!success) return NotFound(new { message = "No existe." });
        return Ok(new { message = "Eliminado correctamente." });
    }
}