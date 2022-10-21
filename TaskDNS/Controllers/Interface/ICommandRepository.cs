using TaskDNS.Models;
using TaskDNS.Models.Dto;

namespace TaskDNS.Controllers.Interface
{
    /// <summary>
    /// Интерфейс указывающий функционал для работы с базой данных.
    /// </summary>
    public interface ICommandRepository
    {
        /// <summary>
        /// Получения списка историй из базы данных.
        /// </summary>
        public IEnumerable<Command> AllHistory();

        /// <summary>
        /// Добавление команды в базу данных.
        /// </summary>
        public void Add(Command command);

        /// <summary>
        /// Удаление из базы данных.
        /// </summary>
        public void Remove(Command command);

        /// <summary>
        /// Сохранение в базу данных.
        /// </summary>
        public void Save();
    }
}
