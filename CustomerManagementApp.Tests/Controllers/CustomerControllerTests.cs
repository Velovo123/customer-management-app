using Xunit;
using Moq;
using CustomerManagementApp.Controllers;
using CustomerManagementApp.Models;
using CustomerManagementApp.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerService> _mockCustomerService;
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _mockCustomerService = new Mock<ICustomerService>();
        _controller = new CustomerController(_mockCustomerService.Object);
    }

    [Fact]
    public async Task Index_ShouldReturnViewWithCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new Customer { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
        };

        _mockCustomerService.Setup(service => service.GetAllAsync()).ReturnsAsync(customers);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(customers, viewResult.Model);
    }

    [Fact]
    public async Task Index_ShouldReturnNoCustomersFoundView_WhenNoCustomersExist()
    {
        // Arrange
        _mockCustomerService.Setup(service => service.GetAllAsync()).ReturnsAsync((List<Customer>?)null);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("NoCustomersFound", viewResult.ViewName);
    }

    [Fact]
    public void Create_Get_ShouldReturnView()
    {
        // Act
        var result = _controller.Create();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Post_ShouldRedirectToIndex_WhenModelIsValid()
    {
        // Arrange
        var customer = new Customer { FirstName = "John", LastName = "Doe" };

        // Act
        var result = await _controller.Create(customer);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockCustomerService.Verify(service => service.CreateAsync(customer), Times.Once);
    }

    [Fact]
    public async Task Create_Post_ShouldReturnView_WhenModelIsInvalid()
    {
        // Arrange
        var customer = new Customer();
        _controller.ModelState.AddModelError("FirstName", "Required");

        // Act
        var result = await _controller.Create(customer);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(customer, viewResult.Model);
    }

    [Fact]
    public async Task Details_ShouldReturnViewWithCustomer_WhenCustomerExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };
        _mockCustomerService.Setup(service => service.GetByIdAsync(customerId)).ReturnsAsync(customer);

        // Act
        var result = await _controller.Details(customerId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(customer, viewResult.Model);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _mockCustomerService.Setup(service => service.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _controller.Details(customerId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Get_ShouldReturnViewWithCustomer_WhenCustomerExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };
        _mockCustomerService.Setup(service => service.GetByIdAsync(customerId)).ReturnsAsync(customer);

        // Act
        var result = await _controller.Edit(customerId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(customer, viewResult.Model);
    }

    [Fact]
    public async Task Edit_Get_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _mockCustomerService.Setup(service => service.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _controller.Edit(customerId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Post_ShouldRedirectToIndex_WhenModelIsValid()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };

        // Act
        var result = await _controller.Edit(customerId, customer);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockCustomerService.Verify(service => service.UpdateAsync(customer), Times.Once);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };

        // Act
        var result = await _controller.Edit(customerId, customer);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Mismatched customer ID.", badRequestResult.Value);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnView_WhenModelIsInvalid()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer { Id = customerId };
        _controller.ModelState.AddModelError("FirstName", "Required");

        // Act
        var result = await _controller.Edit(customerId, customer);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(customer, viewResult.Model);
    }

    [Fact]
    public async Task Delete_Get_ShouldReturnViewWithCustomer_WhenCustomerExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };
        _mockCustomerService.Setup(service => service.GetByIdAsync(customerId)).ReturnsAsync(customer);

        // Act
        var result = await _controller.Delete(customerId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(customer, viewResult.Model);
    }

    [Fact]
    public async Task Delete_Get_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _mockCustomerService.Setup(service => service.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _controller.Delete(customerId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_ShouldRedirectToIndex()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var result = await _controller.DeleteConfirmed(customerId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockCustomerService.Verify(service => service.DeleteAsync(customerId), Times.Once);
    }
}
