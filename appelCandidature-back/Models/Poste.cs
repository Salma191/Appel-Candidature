namespace pfe_back.Models
{
    public class Poste
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Exigence { get; set; }
        public DateTime DatePublication { get; set; }
        public required string NumeroUnique { get; set; }
        public string? Critere { get; set; }
        public ICollection<Candidature>? Candidatures { get; set; }
        public int? PVId { get; set; }
        public PV? PV { get; set; }
        public int? DecisionId { get; set; }
        public Decision? Decision { get; set; }

    }
}
