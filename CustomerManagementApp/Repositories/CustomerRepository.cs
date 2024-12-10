using CustomerManagementApp.Models;
using MongoDB.Driver;
using DotNetEnv;
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

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, id);
            await _customers.DeleteOneAsync(filter);
        }

        public async Task<List<Customer>?> GetAllAsync()
        {
            var customers = await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
            return customers.Count == 0 ? null : customers;
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, id);
            return await _customers.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Customer entity)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, entity.Id);
            var update = Builders<Customer>.Update
                .Set(c => c.FirstName, entity.FirstName)
                .Set(c => c.LastName, entity.LastName)
                .Set(c => c.Email, entity.Email)
                .Set(c => c.PhoneNumber, entity.PhoneNumber)
                .Set(c => c.Address, entity.Address);

            await _customers.UpdateOneAsync(filter, update);
        }
    }
}
