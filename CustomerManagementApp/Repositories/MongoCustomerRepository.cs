using CustomerManagementApp.Models;
using MongoDB.Driver;
using DotNetEnv;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace CustomerManagementApp.Repositories
{
    public class MongoCustomerRepository
    {
        private readonly IMongoCollection<Customer> _customers;

        public MongoCustomerRepository(IMongoClient client, string databaseName)
        {
            var database = client.GetDatabase(databaseName);
            _customers = database.GetCollection<Customer>("Customers");
        }

        public async Task CreateAsync(Customer customer)
        {
            await _customers.InsertOneAsync(customer);
        }

        public async Task<List<Customer>?> GetAllAsync()
        {
            var customers = await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
            return customers.Count == 0 ? null : customers;
        }

    }
}
