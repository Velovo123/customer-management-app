using CustomerManagementApp.Models;

namespace CustomerManagementApp.Repositories.IRepositories
{
    public interface ICustomerRepository
    {
        Task CreateAsync(Customer customer);
        Task<List<Customer>?> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
    }
}
