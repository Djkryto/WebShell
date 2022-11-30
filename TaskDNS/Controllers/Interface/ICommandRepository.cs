using TaskDNS.Models;

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
        public IEnumerable<Command> GetHistory();

        /// <summary>
        /// Добавление команды в базу данных.
        /// </summary>
        public void Add(Command command);

        /// <summary>
        /// Сохранение в базу данных.
        /// </summary>
        public void Save();
    }
}