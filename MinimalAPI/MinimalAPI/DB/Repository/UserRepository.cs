using Microsoft.EntityFrameworkCore;
using MinimalAPI.DB.IRepository;
using MinimalAPI.Model;

namespace MinimalAPI.DB.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly MinimalDbContext _dbContext;
        public UserRepository(MinimalDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<bool> UserExistsByNameAsync(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

    }
}
