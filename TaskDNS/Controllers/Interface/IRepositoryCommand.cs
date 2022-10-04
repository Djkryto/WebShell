using TaskDNS.Models;

namespace TaskDNS.Controllers.Interface
{
    public interface IRepositoryCommand
    {
        public IEnumerable<Command> AllHistory();
        public void Add(Command command);
        public void Remove(Command command);
        public void Save();
    }
}
