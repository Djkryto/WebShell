using TaskDNS.Database.Model;

namespace TaskDNS.Domain.Interface
{
    /// <summary>
    /// Интерфейс для взаимодействия с таблицой токенов в базе данных.
    /// </summary>
    public interface ITokenRepository
    {
        /// <summary>
        /// Получения токе из базы данных.
        /// </summary>
        public Task<IEnumerable<Token>> GetAllTokensAsync();

        /// <summary>
        /// Добавление токена в базу данных.
        /// </summary>
        public Task AddAsync(Token command);

        /// <summary>
        /// Обновление токена в базе данных.
        /// </summary>
        public Task UpdateAsync(int id, string value);

        /// <summary>
        /// Сохранение изменений в базу данных.
        /// </summary>
        public Task SaveAsync();

    }
}