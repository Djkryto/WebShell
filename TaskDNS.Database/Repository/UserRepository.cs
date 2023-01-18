using TaskDNS.Domain.Interface;
using TaskDNS.Database.Model;

namespace TaskDNS.Database.Repository
{
    ///<inheritdoc />
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _db;

        ///<inheritdoc />
        public UserRepository(DBContext db) => _db = db;

        ///<inheritdoc />
        public async Task AddAsync(User user) => await _db.AddAsync(user);

        ///<inheritdoc  />
        public async Task SaveAsync() => await _db.SaveChangesAsync();

        ///<inheritdoc />
        public Task<IEnumerable<User>> GetUsersAsync() => Task.Run<IEnumerable<User>>(() => _db.Users);
    }
}