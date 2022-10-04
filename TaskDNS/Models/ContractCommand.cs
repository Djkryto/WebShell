using System.Runtime.Serialization;
namespace TaskDNS.Models
{
    [DataContract]
    public class ContractCommand
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "data")]
        public string Data { get; set; }
        [DataMember(Name = "textCommand")]
        public string TextCommand { get; set; }
    }
}
