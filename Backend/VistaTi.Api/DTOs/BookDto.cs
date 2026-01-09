namespace VistaTi.Api.DTOs;

public class BookDto
{
    public string ExternalId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<string> Authors { get; set; } = new();
    public string? FirstPublishYear { get; set; }
    public string? CoverUrl { get; set; }
}