using MinimalAPI.Model;

namespace MinimalAPI.DB.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetUserByUsernameAndPasswordAsync(string username, string password);

        public Task<bool> UserExistsByNameAsync(string username);
    }
}
