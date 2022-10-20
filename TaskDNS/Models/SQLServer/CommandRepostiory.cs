using TaskDNS.Controllers.Interface;
using Microsoft.EntityFrameworkCore;
using TaskDNS.Models.Dto;

namespace TaskDNS.Models.SQLServer
{
    ///<inheritdoc cref="ICommandRepository"/>
    public class CommandRepostiory : ICommandRepository
    {
        private CommandContext db;
        ///<inheritdoc cref="ICommandRepository"/>
        public CommandRepostiory(CommandContext db)
        {
            this.db = db;
        }
        ///<inheritdoc cref="ICommandRepository.Add" />
        public void Add(Command command)
        {
           db.Add(command);
        }
        ///<inheritdoc cref="ICommandRepository.Save" />
        public void Save()
        {
            db.SaveChanges();
        }
        ///<inheritdoc cref="ICommandRepository.Remove" />
        public void Remove(Command command)
        {
            db.Remove(command);
        }
        ///<inheritdoc cref="ICommandRepository.AllHistory" />
        public IEnumerable<Command> AllHistory()
        {
           return db.Commands;
        }
    }
}
