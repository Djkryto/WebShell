using Microsoft.EntityFrameworkCore;
using TaskDNS.Database.Model;

namespace TaskDNS.Database
{
    /// <summary>
    /// Класс отвещающий за создание и обращение к базе данных.
    /// </summary>
    public class DBContext : DbContext
    {
        /// <summary>
        /// Таблица команд.
        /// </summary>
        public DbSet<Command> Commands { get; set; }

        /// <summary>
        /// Таблица пользователей.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица токенов.
        /// </summary>
        public DbSet<Token> Tokens { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public DBContext(DbContextOptions<DBContext> options)
            : base(options) => Database.EnsureCreated();

    }
}