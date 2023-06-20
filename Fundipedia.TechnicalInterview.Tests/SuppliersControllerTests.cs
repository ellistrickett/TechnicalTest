using Fundipedia.TechnicalInterview.Model.Supplier;
using Fundipedia.TechnicalInterview.Domain;
using Fundipedia.TechnicalInterview.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fundipedia.TechnicalInterview.Test;
public class SuppliersControllerTests
{
    private readonly Mock<ISupplierService> mockSupplierService;

    public SuppliersControllerTests()
    {
        mockSupplierService = new Mock<ISupplierService>();
    }

    [Fact]
    public async Task GetSuppliers_ReturnsAllSuppliers_WhenSuppliersExist()
    {
        var expectedSuppliers = new List<Supplier>()
        {
            new Supplier { Id = Guid.Parse("160b1ce3-e239-4dae-b971-b60dbff6272f"), FirstName = "John", LastName = "Doe" },
            new Supplier { Id = Guid.Parse("e3274416-b702-4eab-8583-142dc6d080cc"), FirstName = "Jane", LastName = "Smith" }
        };

        mockSupplierService.Setup(service => service.GetSuppliers())
            .ReturnsAsync(expectedSuppliers);

        var controller = new SuppliersController(mockSupplierService.Object);

        var suppliers = await controller.GetSuppliers();

        var result = Assert.IsType<ActionResult<IEnumerable<Supplier>>>(suppliers);
        var actualSuppliers = Assert.IsAssignableFrom<IEnumerable<Supplier>>(result.Value);

        Assert.Equal(expectedSuppliers.Count, actualSuppliers.Count());
        Assert.Equal(expectedSuppliers, actualSuppliers);
    }

    [Fact]
    public async Task GetSuppliers_ReturnsNotFound_WhenSuppliersDoNotExist()
    {
        mockSupplierService.Setup(service => service.GetSuppliers())
            .ReturnsAsync(new List<Supplier>());

        var controller = new SuppliersController(mockSupplierService.Object);

        var actionResult = await controller.GetSuppliers();

        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetSupplier_ReturnsSupplier_WhenSupplierExists()
    {
        mockSupplierService.Setup(service => service.GetSupplier(It.IsAny<Guid>()))
            .ReturnsAsync(new Supplier());

        var controller = new SuppliersController(mockSupplierService.Object);

        var result = await controller.GetSupplier(Guid.NewGuid());

        Assert.IsType<Supplier>(result.Value);
    }

    [Fact]
    public async Task GetSupplier_ReturnsNotFound_WhenSupplierDoesNotExist()
    {
        var nonExistentSupplierId = Guid.NewGuid();
        mockSupplierService.Setup(service => service.GetSupplier(nonExistentSupplierId))
            .ReturnsAsync((Supplier)null);

        var controller = new SuppliersController(mockSupplierService.Object);

        var result = await controller.GetSupplier(nonExistentSupplierId);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostSupplier_ReturnsCreatedAtAction_WhenSupplierValid()
    {
        mockSupplierService.Setup(service => service.InsertSupplier(It.IsAny<Supplier>()))
            .Returns((Supplier supplier) => Task.CompletedTask);

        var controller = new SuppliersController(mockSupplierService.Object);

        var result = await controller.PostSupplier(new Supplier());

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async Task DeleteSupplier_ReturnsSupplier_WhenSupplierValid()
    {
        mockSupplierService.Setup(service => service.DeleteSupplier(It.IsAny<Guid>()))
            .ReturnsAsync(new Supplier());

        var controller = new SuppliersController(mockSupplierService.Object);

        var result = await controller.DeleteSupplier(Guid.NewGuid());

        Assert.IsType<Supplier>(result.Value);
    }

    [Fact]
    public async Task DeleteSupplier_ReturnsNotFound_WhenSupplierDoesNotExist()
    {
        var nonExistentSupplierId = Guid.NewGuid();
        mockSupplierService.Setup(service => service.DeleteSupplier(nonExistentSupplierId))
            .ReturnsAsync((Supplier)null);

        var controller = new SuppliersController(mockSupplierService.Object);

        var result = await controller.DeleteSupplier(nonExistentSupplierId);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
