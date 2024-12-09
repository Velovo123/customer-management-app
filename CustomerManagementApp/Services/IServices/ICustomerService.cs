using CustomerManagementApp.Models;

namespace CustomerManagementApp.Services.IServices
{
    public interface ICustomerService
    {
        Task CreateAsync(Customer customer);
        Task<List<Customer>?> GetAllAsync();
        Task<Customer?> GetByIdAsync(string id);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(string id);
    }
}
