namespace pfe_back.Models
{
    public class PV
    {
        public int Id { get; set; }
        public required string Reference { get; set; }
        public int EntiteId { get; set; }
        public Entite? Entite { get; set; }
        public ICollection<PieceJointe>? PieceJointes { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateValidation { get; set; }
        public string? Statut { get; set; } = "Draft";
        public int TypePosteId { get; set; }
        public TypePoste? TypePoste { get; set; }
        public int CommissionId { get; set; }
        public Commission? Commission { get; set; }
        public Decision? Decision { get; set; }
        public ICollection<Poste>? Postes { get; set; }

    }
}
