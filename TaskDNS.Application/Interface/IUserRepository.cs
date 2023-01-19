using TaskDNS.Database.Model;

namespace TaskDNS.Domain.Interface
{
    /// <summary>
    /// Интерфейс работающий с таблицой пользователей в базе данных.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получения списка историй из базы данных.
        /// </summary>
        public Task<IEnumerable<User>> GetUsersAsync();

        /// <summary>
        /// Добавление команды в базу данных.
        /// </summary>
        public Task AddAsync(User user);

        /// <summary>
        /// Сохранение в базу данных.
        /// </summary>
        public Task SaveAsync();
    }
}