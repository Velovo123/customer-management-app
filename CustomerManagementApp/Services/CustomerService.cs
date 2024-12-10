using CustomerManagementApp.Models;
using CustomerManagementApp.Repositories.IRepositories;
using CustomerManagementApp.Services.IServices;

namespace CustomerManagementApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
            await _repository.CreateAsync(customer);
        }

        public async Task<List<Customer>?> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Customer ID cannot be empty", nameof(id));
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
            if (customer.Id == Guid.Empty) throw new ArgumentException("Customer ID cannot be empty", nameof(customer));
            await _repository.UpdateAsync(customer);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Customer ID cannot be empty", nameof(id));
            await _repository.DeleteAsync(id);
        }
    }
}
