using Moq;
using MongoDB.Driver;
using CustomerManagementApp.Models;
using CustomerManagementApp.Repositories;
using Microsoft.Extensions.Options;


// These tests are not as concise as desired due to challenges with mocking MongoDB's API methods like FindAsync and UpdateOneAsync,
// which require additional setup for IAsyncCursor and filter definitions.
public class CustomerRepositoryTests
{
    private readonly Mock<IMongoClient> _mockMongoClient;
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly Mock<IMongoCollection<Customer>> _mockCollection;
    private readonly CustomerRepository _customerRepository;

    public CustomerRepositoryTests()
    {
        _mockMongoClient = new Mock<IMongoClient>();
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockCollection = new Mock<IMongoCollection<Customer>>();

        var mockMongoSettings = Options.Create(new MongoSettings { DatabaseName = "TestDatabase" });

        _mockMongoClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
            .Returns(_mockDatabase.Object);

        _mockDatabase.Setup(d => d.GetCollection<Customer>(It.IsAny<string>(), null))
            .Returns(_mockCollection.Object);

        _customerRepository = new CustomerRepository(_mockMongoClient.Object, mockMongoSettings);
    }

    private Customer CreateSampleCustomer()
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St"
        };
    }

    [Fact]
    public async Task CreateAsync_ShouldInsertCustomer()
    {
        // Arrange
        var customer = CreateSampleCustomer();

        // Act
        await _customerRepository.CreateAsync(customer);

        // Assert
        _mockCollection.Verify(collection => collection.InsertOneAsync(
            customer,
            null,
            default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDeleteOneAsync()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        await _customerRepository.DeleteAsync(customerId);

        // Assert
        _mockCollection.Verify(
            c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    
    [Fact]
    public async Task GetAllAsync_ShouldCallFindAsync()
    {
        // Arrange
        var mockCursor = new Mock<IAsyncCursor<Customer>>();
        mockCursor.Setup(cursor => cursor.MoveNext(It.IsAny<CancellationToken>())).Returns(true);
        mockCursor.Setup(cursor => cursor.Current).Returns(new List<Customer>());

        _mockCollection.Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<FindOptions<Customer>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        await _customerRepository.GetAllAsync();

        // Assert
        _mockCollection.Verify(
            c => c.FindAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<FindOptions<Customer>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task UpdateAsync_ShouldCallUpdateOneAsync()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St"
        };

        _mockCollection.Setup(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<UpdateDefinition<Customer>>(),
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<UpdateResult>());

        // Act
        await _customerRepository.UpdateAsync(customer);

        // Assert
        _mockCollection.Verify(
            c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<UpdateDefinition<Customer>>(),
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }




}
