namespace CustomerManagementApp.Repositories.IRepositories
{
    public interface IMongoCustomerRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task<List<T>?> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
