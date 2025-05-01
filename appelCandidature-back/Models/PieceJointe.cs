using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class PieceJointe
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Fichier { get; set; }
        public TypePJ? Type { get; set; }
        public TypeDoc? TypeDoc { get; set; }
        public int? PVId { get; set; }
        [JsonIgnore]
        public PV? PV { get; set; }
        public int? DecisionId { get; set; }
        [JsonIgnore]
        public Decision? Decision { get; set; }
        public int? CandidatureId { get; set; }
        [JsonIgnore]
        public Candidature? Candidature { get; set; }
        public bool IsFromPV { get; set; } = false;

    }

    public enum TypePJ
    {
        Signée,
        Supp,
        Candidature
    }

    public enum TypeDoc
    {
        CV,
        CL
    }
}
