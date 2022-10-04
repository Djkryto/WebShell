using Microsoft;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TaskDNS.Models.SQLServer
{
    public class CommandContext : DbContext
    {
        public DbSet<Command> Commands { get; set; } = null!;

        public CommandContext(DbContextOptions<CommandContext> options)
            : base(options)
        {
            Database.EnsureCreated(); // создаем базу данных при первом обращении
        }
    }
}
