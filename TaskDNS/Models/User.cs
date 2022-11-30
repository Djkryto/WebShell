using System.ComponentModel.DataAnnotations;

namespace TaskDNS.Models
{
    public class User
    {
        [Key]
        public int Id { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
        public string Token { get; set; }
    }
}
