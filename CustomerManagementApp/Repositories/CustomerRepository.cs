using CustomerManagementApp.Models;
using MongoDB.Driver;
using DotNetEnv;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using CustomerManagementApp.Repositories.IRepositories;
using Microsoft.Extensions.Options;

namespace CustomerManagementApp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<Customer> _customers;

        public CustomerRepository(IMongoClient client, IOptions<MongoSettings> mongoSettings)
        {
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _customers = database.GetCollection<Customer>("Customers");
        }

        public async Task CreateAsync(Customer customer)
        {
            await _customers.InsertOneAsync(customer);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Customer>.Filter.Eq("Id", id);
            await _customers.DeleteOneAsync(filter);
        }

        public async Task<List<Customer>?> GetAllAsync()
        {
            var customers = await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
            return customers.Count == 0 ? null : customers;
        }

        public async Task<Customer?> GetByIdAsync(string id)
        {
            var filter = Builders<Customer>.Filter.Eq("Id", id);
            return await _customers.Find(filter).FirstOrDefaultAsync();
        }

        public Task UpdateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
