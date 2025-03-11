using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class MembreCommission
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoleCommission RoleCommission { get; set; }

        public int CommissionId { get; set; }
        [JsonIgnore]
        public Commission? Commission { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur? Utilisateur { get; set; }

    }

    public enum RoleCommission
    {
        Président,
        [Display(Name = "Secrétariat DAO")]
        Secrétariat_DAO,
        Membre,
        [Display(Name = "Secrétariat DAO & Membre")]
        Secrétariat_DAO_Membre
    }
}
