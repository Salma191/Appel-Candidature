namespace pfe_back.Models
{
    public class DAO
    {
        public int Id { get; set; }
        public ICollection<PV>? PVs { get; set; }
        public ICollection<Decision>? Decisions { get; set; }
    }
}
