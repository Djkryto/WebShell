using TaskDNS.Database.Model;

namespace TaskDNS.Network.Dto
{
    public class ClientDataDto
    {
        public User User { get; init; }

        public bool IsJWT { get; init; }

        public ClientDataDto(User user, bool isJWT)
        {
            User = user;
            IsJWT = isJWT;
        }
    }
}