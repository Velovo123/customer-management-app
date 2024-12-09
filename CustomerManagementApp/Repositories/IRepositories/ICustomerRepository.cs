using CustomerManagementApp.Models;

namespace CustomerManagementApp.Repositories.IRepositories
{
    public interface ICustomerRepository
    {
        Task CreateAsync(Customer customer);
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(string id);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(string id);
    }
}
