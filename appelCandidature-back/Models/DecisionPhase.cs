namespace pfe_back.Models
{
    public class DecisionPhase
    {
        public int DecisionId { get; set; }
        public Decision? Decision { get; set; }
        public int PhaseId { get; set; }
        public Phase? Phase { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? Statut { get; set; }
    }
}
