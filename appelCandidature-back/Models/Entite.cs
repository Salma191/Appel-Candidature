using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class Entite
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        [JsonIgnore]
        public ICollection<Poste>? Postes { get; set; }
        [JsonIgnore]
        public ICollection<PV>? PVs { get; set; }
        [JsonIgnore]
        public ICollection<Decision>? Decisions { get; set; }
    }
}
