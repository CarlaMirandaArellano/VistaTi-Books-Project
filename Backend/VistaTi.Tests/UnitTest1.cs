using Xunit;
using Microsoft.EntityFrameworkCore;
using VistaTi.Api.Data;
using VistaTi.Api.Models;
using VistaTi.Api.Services;

namespace VistaTi.Tests;

public class UnitTest1
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    // 1. Evitar duplicados (YA FUNCIONA)
    [Fact]
    public async Task Test_AgregarFavorito_NoDuplicados()
    {
        using var context = GetDbContext();
        var service = new BookService(context, null!); // El ! quita el warning
        var book = new Favorite { ExternalId = "123", UserId = 1, Title = "Libro Test" };

        await service.AddFavoriteAsync(book);
        var result = await service.AddFavoriteAsync(book); 

        Assert.False(result);
    }

    // 2. Validación de campos faltantes
    [Fact]
    public async Task Test_Validacion_CamposFaltantes()
    {
        using var context = GetDbContext();
        var service = new BookService(context, null!);
        var invalidBook = new Favorite { UserId = 1 }; // Falta Title y ExternalId

        await Assert.ThrowsAsync<ArgumentException>(() => service.AddFavoriteAsync(invalidBook));
    }

    // 3. Normalización/Mapeo de ID
    [Fact]
    public void Test_Mapeo_NormalizacionID()
    {
        var externalKey = "/works/OL123W";
        var normalizedId = externalKey.Replace("/works/", "");
        
        Assert.Equal("OL123W", normalizedId);
    }

    // 4. Eliminar favorito existente
    [Fact]
    public async Task Test_Eliminar_Existente()
    {
        using var context = GetDbContext();
        var service = new BookService(context, null!);
        var book = new Favorite { ExternalId = "DEL-1", Title = "Eliminar", UserId = 1 };
        context.Favorites.Add(book);
        await context.SaveChangesAsync();

        var result = await service.DeleteFavoriteAsync(book.Id);

        Assert.True(result);
        Assert.Empty(context.Favorites);
    }

    // 5. Eliminar favorito inexistente
    [Fact]
    public async Task Test_Eliminar_Inexistente()
    {
        using var context = GetDbContext();
        var service = new BookService(context, null!);

        var result = await service.DeleteFavoriteAsync(999); 

        Assert.False(result);
    }
}