using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TiendaDeportiva.Models
{
    public enum PersonType
    {
        [EnumMember(Value = "Cliente")]
        Cliente = 0,

        [EnumMember(Value = "Administrador")]
        Administrador = 1
    }
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonProperty("Type")]
        public PersonType Type { get; set; }
    }
}