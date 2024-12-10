using CustomerManagementApp.Models;
using CustomerManagementApp.Repositories.IRepositories;
using CustomerManagementApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerManagementApp.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _service = new CustomerService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallCreateOnRepository()
        {
            // Arrange
            var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            // Act
            await _service.CreateAsync(customer);

            // Assert
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenCustomerIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCustomers_WhenCustomersExist()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com" },
                new Customer { FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com" }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.FirstName == "Alice");
            Assert.Contains(result, c => c.FirstName == "Bob");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnNull_WhenNoCustomersExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync((List<Customer>?)null);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };
            _mockRepository.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);

            // Act
            var result = await _service.GetByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result?.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetByIdAsync(Guid.Empty));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Customer?)null);

            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdateOnRepository()
        {
            // Arrange
            var customer = new Customer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };

            // Act
            await _service.UpdateAsync(customer);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentNullException_WhenCustomerIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenCustomerIdIsEmpty()
        {
            // Arrange
            var customer = new Customer { Id = Guid.Empty, FirstName = "John", LastName = "Doe" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(customer));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteOnRepository()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(customerId);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteAsync(Guid.Empty));
        }
    }
}
