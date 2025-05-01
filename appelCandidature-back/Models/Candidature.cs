namespace pfe_back.Models
{
    public class Candidature
    {
        public int Id { get; set; }
        public string? Statut { get; set; } = "Brouillon";
        public bool EstComplete { get; set; } = false;

        public string? NumeroEnregistrement { get; set; }
        public DateTime DatePostulation { get; set; }
        public int CandidatId { get; set; }
        public Candidat? Candidat { get; set; }
        public ICollection<PieceJointe>? PieceJointes { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public int PosteId { get; set; }
        public Poste? Poste { get; set; }
        public int DecisionId { get; set; }
        public Decision? Decision { get; set; }
    }
}
