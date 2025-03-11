namespace pfe_back.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Lien { get; set; }
        public string? Type { get; set; }
        public int CandidatureId { get; set; }
        public Candidature? Candidature { get; set; }
    }
}
