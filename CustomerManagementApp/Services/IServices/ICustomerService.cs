using CustomerManagementApp.Models;

namespace CustomerManagementApp.Services.IServices
{
    public interface ICustomerService
    {
        Task CreateAsync(Customer customer);
        Task<List<Customer>?> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid id);
    }
}
