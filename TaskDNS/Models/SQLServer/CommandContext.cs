using Microsoft.EntityFrameworkCore;

namespace TaskDNS.Models.SQLServer
{
    /// <summary>
    /// Класс отвещающий за создание и обращение к базе данных.
    /// </summary>
    public class CommandContext : DbContext
    {
        /// <summary>
        /// Сущность для работы с базой данных.
        /// </summary>
        public DbSet<Command> Commands { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public CommandContext(DbContextOptions<CommandContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
