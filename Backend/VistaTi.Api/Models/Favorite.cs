using System.ComponentModel.DataAnnotations;

namespace VistaTi.Api.Models;

public class Favorite
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ExternalId { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Authors { get; set; } // Guardaremos los autores separados por comas

    public string? FirstPublishYear { get; set; }

    public string? CoverUrl { get; set; }

    [Required]
    public int UserId { get; set; } = 1; // Usuario fijo UserId=1 por requerimiento
}