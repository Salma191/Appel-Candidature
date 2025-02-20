namespace pfe_back.Models
{
    public class DAO : Utilisateur
    {
        public ICollection<PV> PVs { get; set; }
        public ICollection<Decision> Decisions { get; set; }
    }
}
