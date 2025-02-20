using System.ComponentModel.DataAnnotations;

namespace pfe_back.Models
{
    public class MembreCommission : Utilisateur
    {
        public RoleCommission Role { get; set; }

        public int CommissionId { get; set; }
        public Commission? Commission { get; set; }
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
