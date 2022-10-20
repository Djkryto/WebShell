using TaskDNS.Controllers.Interface;
using Microsoft.EntityFrameworkCore;

namespace TaskDNS.Models.SQLServer
{
    /// <summary>
    /// Класс реализующий взаимодействие с базой данных.
    /// </summary>
    public class CommandRepostiory : ICommandRepository
    {
        private CommandContext db;
        /// <summary>
        /// Получение базы данных.
        /// </summary>
        public CommandRepostiory(CommandContext db)
        {
            this.db = db;
        }
        /// <summary>
        /// Добавление команды в базу данных.
        /// </summary>
        public void Add(Command command)
        {
           db.Add(command);
        }
        /// <summary>
        /// Сохранение в базу данных.
        /// </summary>
        public void Save()
        {
            db.SaveChanges();
        }
        /// <summary>
        /// Удаление из базы данных.
        /// </summary>
        public void Remove(Command command)
        {
            db.Remove(command);
        }
        /// <summary>
        /// Получение полного списка истории команд из базы данных.
        /// </summary>
        public IEnumerable<Command> AllHistory()
        {
           return db.Commands;
        }
    }
}
