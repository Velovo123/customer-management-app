using Xunit;
using Moq;
using MongoDB.Driver;
using CustomerManagementApp.Models;
using CustomerManagementApp.Repositories;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson.Serialization;

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

    
}
