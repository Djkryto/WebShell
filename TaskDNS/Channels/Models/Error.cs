using TaskDNS.Channels.Interface;
using TaskDNS.Controllers.Interface;

namespace TaskDNS.Channels.Models
{ /// <summary>
  /// Класс со статусом выполненой команды с ошибкой.
  /// </summary>
    public class Error : ICMDCommand
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
        /// Конструкто определяющий значения свойств Output и Status.
        /// </summary>
        /// <param name="output">Ответ с сервера.</param>
        public Error(string output)
        {
            Status = Status.Error;
            Output = output;
        }
    }
}
