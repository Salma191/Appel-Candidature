using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class Decision
    {
        public int Id { get; set; }
        public string? Reference { get; set; }
        public int EntiteId { get; set; }
        public Entite? Entite { get; set; }
        public string? Statut { get; set; } = "Draft";
        public DateTime DateCreation { get; set; }
        public DateTime? DateSignature { get; set; }
        public DateTime? DatePublication { get; set; }
        public DateTime? DateLimite { get; set; }
        public int NbreMaxPosteCandidat { get; set; }
        public ICollection<PieceJointe>? PieceJointes { get; set; }
        public int PVId { get; set; }
        public PV? PV { get; set; }
        public ICollection<Poste>? Postes { get; set; }
        [JsonIgnore]
        public ICollection<Candidature>? Candidatures { get; set; }
        [JsonIgnore]
        public ICollection<DecisionPhase> DecisionPhases { get; set; } = new List<DecisionPhase>();

    }
}
