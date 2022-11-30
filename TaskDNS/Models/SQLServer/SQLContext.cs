using Microsoft.EntityFrameworkCore;

namespace TaskDNS.Models.SQLServer
{
    /// <summary>
    /// Класс отвещающий за создание и обращение к базе данных.
    /// </summary>
    public class SQLContext : DbContext
    {
        /// <summary>
        /// Сущность комманда для работы с базой данных.
        /// </summary>
        public DbSet<Command> Commands { get; set; }

        /// <summary>
        /// Сущность пользователи для работы с базой данных.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public SQLContext(DbContextOptions<SQLContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
