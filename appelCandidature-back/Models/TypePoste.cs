namespace pfe_back.Models
{
    public class TypePoste
    {
        public int Id { get; set; }
        public required string Nom { get; set; }
        public ICollection<PV>? PVs { get; set; }
    }
}
