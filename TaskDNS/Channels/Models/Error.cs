using TaskDNS.Channels.Interface;

namespace TaskDNS.Channels.Models
{    
     /// <summary>
     /// Класс со статусом выполненой команды с ошибкой.
     /// </summary>
    public class Error : ICommandWithStatus
    {   
         /// <summary>
         /// Статус в данном классе имеет значение Error. 
         /// </summary>
        public Status Status { get;}

        /// <summary>
        /// Ответ с сервера. 
        /// </summary>
        public string Output { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        public Error(string output)
        {
            Status = Status.Error;
            Output = output;
        }
    }
}
