namespace pfe_back.Models
{
    public class Candidat
    {
        public int Id { get; set; }
        public string? AffectationActuelle { get; set; }
        public required DateTime DateRetraite { get; set; }
        public required int JoursAbsence { get; set; }
        public required string Sanction { get; set; }
        public required string NoteTroisDernieresAnnees { get; set; }
        public required string Catégorie { get; set; }
        public required string Congé { get; set; }
        public required string PosteOccupe { get; set; }
        public bool Consentement { get; set; }
        public ICollection<Diplome>?  Diplomes { get; set; }  
        public ICollection<Experience>? Experiences { get; set; }
        public ICollection<Candidature>? Candidatures { get; set; }
        public int UtilisateurId { get; set; }
        public Utilisateur? Utilisateur { get; set; }
    }
}
