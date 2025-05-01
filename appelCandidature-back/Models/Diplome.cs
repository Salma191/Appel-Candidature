using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class Diplome
    {
        public int Id { get; set; }
        public string? Intitulé { get; set; }
        public string? Etablissement { get; set; }
        public int AnnéeObtention { get; set; }
        public string? Niveau { get; set; }
        public int CandidatId { get; set; }
        [JsonIgnore]
        public Candidat? Candidat { get; set; }

    }
}
