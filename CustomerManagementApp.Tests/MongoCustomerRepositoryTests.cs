using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerManagementApp.Models;
using CustomerManagementApp.Repositories;
using MongoDB.Driver;
using Xunit;

namespace CustomerManagementApp.Tests
{
    public class MongoCustomerRepositoryTests : IDisposable
    {
        private readonly MongoCustomerRepository _repository;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoCustomerRepositoryTests()
        {
            var connectionString = "mongodb://localhost:27017";
            var databaseName = "CustomerManagementTestDb";

            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);

            Environment.SetEnvironmentVariable("MONGO_CONNECTION_STRING", connectionString);
            Environment.SetEnvironmentVariable("MONGO_DATABASE_NAME", databaseName);

            _repository = new MongoCustomerRepository();
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
            var customers = _database.GetCollection<Customer>("Customers");
            var savedCustomer = await customers.Find(c => c.Email == "john.doe@example.com").FirstOrDefaultAsync();

            Assert.NotNull(savedCustomer);
            Assert.Equal(customer.FirstName, savedCustomer.FirstName);
        }

        public void Dispose()
        {
            _client.DropDatabase("CustomerManagementTestDb");
        }
    }
}
