namespace TaskDNS.Models.Dto
{
    public class CommandDto
    {
        public  string textCommand { get;}
        public CommandDto(Command command)
        {
            textCommand = command.TextCommand;
        }
    }
}
