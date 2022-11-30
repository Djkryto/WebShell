using TaskDNS.Controllers.Interface;

namespace TaskDNS.Models.SQLServer.Repository
{
    ///<inheritdoc />
    public class CommandRepository : ICommandRepository
    {
        private readonly SQLContext _db;
        ///<inheritdoc />
        public CommandRepository(SQLContext db)
        {
            _db = db;
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
