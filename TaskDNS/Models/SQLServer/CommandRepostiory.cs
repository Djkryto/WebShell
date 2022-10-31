using TaskDNS.Controllers.Interface;

namespace TaskDNS.Models.SQLServer
{
    ///<inheritdoc />
    public class CommandRepostiory : ICommandRepository
    {
        private readonly CommandContext _db;
        ///<inheritdoc />
        public CommandRepostiory(CommandContext db)
        {
            this._db = db;
        }

        ///<inheritdoc />
        public void Add(Command command)
        {
           _db.Add(command);
        }

        ///<inheritdoc  />
        public void Save()
        {
            _db.SaveChanges();
        }
        
        ///<inheritdoc />
        public IEnumerable<Command> GetHistory()
        {
           return _db.Commands;
        }
    }
}
