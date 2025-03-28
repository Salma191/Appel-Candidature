using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class Poste
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Exigence { get; set; }
        public DateTime DatePublication { get; set; }
        public string? NumeroUnique { get; set; }
        public string? Critere { get; set; }
        public int TypePosteId { get; set; }
        public TypePoste? TypePoste { get; set; }
        public ICollection<Candidature>? Candidatures { get; set; }
        [JsonIgnore]
        public ICollection<PV>? PVs { get; set; }
        [JsonIgnore]
        public ICollection<Decision>? Decisions { get; set; }

    }
}
