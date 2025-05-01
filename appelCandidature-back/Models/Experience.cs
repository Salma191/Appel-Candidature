using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string? Poste { get; set; }
        public string? Entreprise { get; set; }
        public DateTime DateDébut { get; set; }
        public DateTime DateFin { get; set; }
        public string? Description { get; set; }
        public int CandidatId { get; set; }
        [JsonIgnore]
        public Candidat? Candidat { get; set; }
    }
}
