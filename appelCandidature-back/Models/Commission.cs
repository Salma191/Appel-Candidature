using System.ComponentModel.DataAnnotations;

namespace pfe_back.Models
{
    public class Commission
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Nom { get; set; }
        [Required]
        public ICollection<MembreCommission> MembreCommissions { get; set; }
        public ICollection<PV> PVs { get; set; }
    }
}
