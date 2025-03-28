using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class Commission
    {
        [Key]
        public int Id { get; set; }
        public string? Nom { get; set; }
        public ICollection<MembreCommission>? MembreCommissions { get; set; }
        [JsonIgnore]
        public ICollection<PV>? PVs { get; set; }
    }
}
