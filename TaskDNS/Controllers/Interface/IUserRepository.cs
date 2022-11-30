using TaskDNS.Models;

namespace TaskDNS.Controllers.Interface
{
    public interface IUserRepository
    {
        /// <summary>
        /// Получения списка историй из базы данных.
        /// </summary>
        public IEnumerable<User> GetUsers();

        /// <summary>
        /// Добавление команды в базу данных.
        /// </summary>
        public void Add(User user);

        /// <summary>
        /// Запись токена пользователю.
        /// </summary>
        public void WriteToken(int id,string token);

        /// <summary>
        /// Сохранение в базу данных.
        /// </summary>
        public void Save();
    }
}
