using TaskDNS.Domain.Interface;
using TaskDNS.Database.Model;

namespace TaskDNS.Database.Repository
{
    ///<inheritdoc />
    public class CommandRepository : ICommandRepository
    {
        private readonly DBContext _db;
        ///<inheritdoc />
        public CommandRepository(DBContext db) => _db = db;

        ///<inheritdoc />
        public async Task AddAsync(Command command) => await _db.AddAsync(command);

        ///<inheritdoc />
        public Task RemoveAsync(string connectionIdHub) => Task.Run(() => _db.RemoveRange(_db.Commands.Where(x => x.IdUserInHub == connectionIdHub)));

        ///<inheritdoc  />
        public async Task SaveAsync() => await _db.SaveChangesAsync();

        ///<inheritdoc />
        public Task<IEnumerable<Command>> GetHistoryUserAsync(string connectionId) => Task.Run<IEnumerable<Command>>(() => _db.Commands.Where(x => x.IdUserInHub == connectionId));
    }
}