using TaskDNS.Domain.Interface;
using TaskDNS.Database.Model;

namespace TaskDNS.Database.Repository
{
    ///<inheritdoc />
    public class TokenRepository : ITokenRepository
    {
        private readonly DBContext _db;

        /// <summary>
        /// .ctor.
        /// </summary>
        /// <param name="context">Модель базы данных.</param>
        public TokenRepository(DBContext context) => _db = context;
        ///<inheritdoc />
        public Task AddAsync(Token command) => Task.Run(() => _db.Add(command));
        ///<inheritdoc />
        public Task SaveAsync() => Task.Run(() => _db.SaveChanges());
        ///<inheritdoc />
        public Task UpdateAsync(int id, string value)
        {
            if (!_db.Tokens.Any(u => u.Id == id))
                return Task.CompletedTask;

            return Task.Run(() =>
            {
                var token = _db.Tokens.First(u => u.Id == id);
                token.Value = value;
                _db.SaveChanges();
            });
        }
        ///<inheritdoc />
        public Task<IEnumerable<Token>> GetAllTokensAsync() => Task.Run<IEnumerable<Token>>(() => _db.Tokens);
    }
}