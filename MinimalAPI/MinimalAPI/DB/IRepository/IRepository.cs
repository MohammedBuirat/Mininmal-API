namespace MinimalAPI.DB.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task AddAsync(T entity);

        public Task UpdateAsync(T entity);

        public Task DeleteAsync(int id);

        public Task<T?> GetByIdAsync(int id);

        public Task<bool> ExistsAsync(int id);

        public Task<List<T>> GetAllAsync();
    }
}
