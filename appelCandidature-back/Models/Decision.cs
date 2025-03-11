namespace pfe_back.Models
{
    public class Decision
    {
        public int Id { get; set; }
        public required string Entite { get; set; }
        public required string Statut { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateSignature { get; set; }
        public DateTime DatePublication { get; set; }
        public DateTime DateLimite { get; set; }
        public int NbreMaxPosteCandidat { get; set; }
        public string PieceJoint { get; set; } = "Draft";
        public int DAOId { get; set; }
        public DAO? DAO { get; set; }
        public int PVId { get; set; }
        public PV? PV { get; set; }
        public ICollection<Phase>? Phases { get; set; }
        public ICollection<Poste>? Postes { get; set; }

    }
}
