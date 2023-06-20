using Fundipedia.TechnicalInterview.Model.Supplier;
using Fundipedia.TechnicalInterview.Data.Context;
using Fundipedia.TechnicalInterview.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fundipedia.TechnicalInterview.Test;
public class SupplierServiceTests : IDisposable
{
    private readonly DbContextOptions<SupplierContext> _options;
    private readonly SupplierContext _context;
    private readonly ISupplierService _service;

    public SupplierServiceTests()
    {
        _options = new DbContextOptionsBuilder<SupplierContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new SupplierContext(_options);

        _context.Suppliers.AddRange(
                new List<Supplier>()
                {
                    new Supplier { Id = Guid.Parse("160b1ce3-e239-4dae-b971-b60dbff6272f"), FirstName = "John", LastName = "Doe" },
                    new Supplier { Id = Guid.Parse("e3274416-b702-4eab-8583-142dc6d080cc"), FirstName = "Jane", LastName = "Smith" }
                }
            );

        _context.SaveChanges();

        _service = new SupplierService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetSupplier_ExistingId_ReturnsSupplier()
    {
        var supplierId = Guid.Parse("160b1ce3-e239-4dae-b971-b60dbff6272f");

        var result = await _service.GetSupplier(supplierId);

        Assert.Equal(supplierId, result.Id);
    }

    [Fact]
    public async Task GetSupplier_NonExistingId_ReturnsNull()
    {
        var supplierId = Guid.NewGuid();

        var result = await _service.GetSupplier(supplierId);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetSuppliers_ReturnsAllSuppliers()
    {
        var result = await _service.GetSuppliers();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task InsertSupplier_SavesSupplierToDatabase()
    {
        var supplier = new Supplier { Id = Guid.NewGuid() };

        await _service.InsertSupplier(supplier);

        var result = await _context.Suppliers.FindAsync(supplier.Id);
        Assert.NotNull(result);
        Assert.Equal(supplier.Id, result.Id);
    }

    [Fact]
    public async Task DeleteSupplier_ExistingId_DeletesSupplier()
    {
        var supplierId = Guid.Parse("160b1ce3-e239-4dae-b971-b60dbff6272f");

        var result = await _service.DeleteSupplier(supplierId);

        Assert.Equal(supplierId, result.Id);
        Assert.Null(await _context.Suppliers.FindAsync(supplierId));
    }

    [Fact]
    public async Task DeleteSupplier_NonExistingId_ReturnsNull()
    {
        var supplierId = Guid.NewGuid();

        var result = await _service.DeleteSupplier(supplierId);

        Assert.Null(result);
    }
}