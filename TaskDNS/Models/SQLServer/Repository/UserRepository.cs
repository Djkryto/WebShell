using TaskDNS.Controllers.Interface;

namespace TaskDNS.Models.SQLServer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLContext _db;
        ///<inheritdoc />
        public UserRepository(SQLContext db)
        {
            _db = db;
        }

        ///<inheritdoc />
        public void Add(User user)
        {
            _db.Add(user);
        }

        ///<inheritdoc  />
        public void Save()
        {
            _db.SaveChanges();
        }

        ///<inheritdoc />
        public IEnumerable<User> GetUsers()
        {
            return _db.Users;
        }

        ///<inheritdoc />
        public void WriteToken(int id, string token)
        {
           var user = _db.Users.First(u => u.Id == id);
           user.Token = token;
           _db.SaveChanges();
        }
    }
}
