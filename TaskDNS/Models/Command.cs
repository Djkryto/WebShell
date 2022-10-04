namespace TaskDNS.Models
{
    public class Command
    {
        public Command(int id, string data, string textCommand)
        {
            Id = id;
            Data = data;
            TextCommand = textCommand;
        }

        public int Id { get; set; }
        public string Data { get; set; }
        public string TextCommand { get; set; }
    }
}
