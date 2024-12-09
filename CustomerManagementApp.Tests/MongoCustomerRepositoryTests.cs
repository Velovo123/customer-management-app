using Moq;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using CustomerManagementApp.Models;
using CustomerManagementApp.Repositories;

public class MongoCustomerRepositoryTests
{
    private readonly Mock<IMongoCollection<Customer>> _mockCollection;
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly Mock<IMongoClient> _mockClient;
    private readonly MongoCustomerRepository _repository;

    public MongoCustomerRepositoryTests()
    {
        // Mock IMongoCollection<Customer>
        _mockCollection = new Mock<IMongoCollection<Customer>>();

        // Mock IMongoDatabase to return the mock collection when GetCollection is called
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockDatabase.Setup(db => db.GetCollection<Customer>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                     .Returns(_mockCollection.Object);

        // Mock IMongoClient to return the mock database when GetDatabase is called
        _mockClient = new Mock<IMongoClient>();
        _mockClient.Setup(client => client.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
                   .Returns(_mockDatabase.Object);

        // Initialize the repository with the mock client and database name
        _repository = new MongoCustomerRepository(_mockClient.Object, "TestDb");
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCustomerToDatabase()
    {
        // Arrange
        var customer = new Customer
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St"
        };

        // Act
        await _repository.CreateAsync(customer);

        // Assert
        _mockCollection.Verify(c => c.InsertOneAsync(It.IsAny<Customer>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    //todo
    [Fact]
    public async Task GetAllAsync_ShouldReturnNull_WhenNoCustomersInDatabase()
    {
        throw new NotImplementedException();
    }


    //todo
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCustomers_WhenCustomersExist()
    {
        throw new NotImplementedException();
    }
}
