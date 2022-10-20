using Microsoft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TaskDNS.Models.Dto;

namespace TaskDNS.Models.SQLServer
{
    /// <summary>
    /// Класс отвещающий за создание и обращение к базе данных.
    /// </summary>
    public class CommandContext : DbContext
    {
        /// <summary>
        /// Таблица с командами.
        /// </summary>
        public DbSet<Command> Commands { get; set; }

        /// <summary>
        /// Образение к базе данных.
        /// </summary>
        public CommandContext(DbContextOptions<CommandContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
