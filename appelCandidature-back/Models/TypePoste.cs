using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class TypePoste
    {
        public int Id { get; set; }
        public required string Nom { get; set; }
        [JsonIgnore]
        public ICollection<Poste>? Postes { get; set; }
    }
}
