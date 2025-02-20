namespace pfe_back.Models
{
    public class PV
    {
        public int Id { get; set; }
        public string Entite { get; set; }
        public string PieceJointe { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateValidation { get; set; }
        public string Statut { get; set; } = "Draft";
        public int TypePosteId { get; set; }
        public TypePoste TypePoste { get; set; }
        public int DAOId { get; set; }
        public DAO DAO { get; set; }
        public int CommissionId { get; set; }
        public Commission Commission { get; set; }
        public Decision Decision { get; set; }
        public ICollection<Poste> Postes { get; set; }

    }
}
