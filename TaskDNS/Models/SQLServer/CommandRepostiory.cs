using TaskDNS.Controllers.Interface;
using Microsoft.EntityFrameworkCore;
using TaskDNS.Models.Dto;

namespace TaskDNS.Models.SQLServer
{
    ///<inheritdoc />
    public class CommandRepostiory : ICommandRepository
    {
        private CommandContext db;
        ///<inheritdoc />
        public CommandRepostiory(CommandContext db)
        {
            this.db = db;
        }

        ///<inheritdoc />
        public void Add(Command command)
        {
           db.Add(command);
        }

        ///<inheritdoc  />
        public void Save()
        {
            db.SaveChanges();
        }

        ///<inheritdoc />
        public void Remove(Command command)
        {
            db.Remove(command);
        }

        ///<inheritdoc />
        public IEnumerable<Command> AllHistory()
        {
           return db.Commands;
        }
    }
}
