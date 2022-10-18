using TaskDNS.Controllers.Interface;
using Microsoft.EntityFrameworkCore;

namespace TaskDNS.Models.SQLServer
{
    public class SQLCommand : IRepositoryCommand
    {
        private CommandContext db;
        public SQLCommand(CommandContext db)
        {
            this.db = db;
        }

        public void Add(Command command)
        {
           db.Add(command);
        }

        public void Save()
        {
            db.SaveChanges();
        }


        public void Remove(Command command)
        {
            db.Remove(command);
        }

        public IEnumerable<Command> AllHistory()
        {
           return db.Commands;
        }
    }
}
