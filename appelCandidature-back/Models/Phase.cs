namespace pfe_back.Models
{
    public class Phase
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Statut { get; set; }
        public int DecisionId { get; set; }
        public Decision Decision { get; set; }
    }
}
