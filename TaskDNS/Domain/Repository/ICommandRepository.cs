using TaskDNS.Database.Model;

namespace TaskDNS.Domain.Interface
{
    /// <summary>
    /// Интерфейс взаимодействующий с таблицой истории пользователей в базе данных.
    /// </summary>
    public interface ICommandRepository
    {
        /// <summary>
        /// Получения списка историй из базы данных.
        /// </summary>
        public Task<IEnumerable<Command>> GetHistoryUserAsync(string connectionId);

        /// <summary>
        /// Добавление команды в базу данных.
        /// </summary>
        public Task AddAsync(Command command);

        /// <summary>
        /// Удаление истории пользователя из базы данных.
        /// </summary>
        public Task RemoveAsync(string connectionIdHub);

        /// <summary>
        /// Сохранение в базу данных.
        /// </summary>
        public Task SaveAsync();
    }
}