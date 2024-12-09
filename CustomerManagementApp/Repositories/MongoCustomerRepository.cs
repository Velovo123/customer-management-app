using CustomerManagementApp.Models;
using MongoDB.Driver;

namespace CustomerManagementApp.Repositories
{
    public class MongoCustomerRepository
    {
        private readonly IMongoCollection<Customer> _customers;

        public MongoCustomerRepository()
        {
            Env.Load();

            var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
            var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME");

            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(databaseName))
            {
                throw new InvalidOperationException("MongoDB connection settings are not set in the environment variables.");
            }

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _customers = database.GetCollection<Customer>("Customers");
        }

        public async Task CreateAsync(Customer customer)
        {
            await _customers.InsertOneAsync(customer);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _customers.Find(_ => true).ToListAsync();
        }
    }
}
