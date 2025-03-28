namespace pfe_back.Models
{
    public class Candidat
    {
        public int Id { get; set; }
        public string? AffectationActuelle { get; set; }
        public DateTime DateRetraite { get; set; }
        public int JoursAbsence { get; set; }
        public string? Sanction { get; set; }
        public string? NoteTroisDernieresAnnees { get; set; }
        public string? Catégorie { get; set; }
        public string? Congé { get; set; }
        public string? PosteOccupe { get; set; }
        public bool Consentement { get; set; }
        public ICollection<Diplome>?  Diplomes { get; set; }  
        public ICollection<Experience>? Experiences { get; set; }
        public ICollection<Candidature>? Candidatures { get; set; }
        public int UtilisateurId { get; set; }
        public Utilisateur? Utilisateur { get; set; }
    }
}
