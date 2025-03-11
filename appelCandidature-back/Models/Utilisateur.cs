using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class Utilisateur
    {
        [Key]
        public int Id { get; set; }

        public string? Nom { get; set; }
        public string? Prenom { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        public string? Password { get; set; }

        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public Candidat? Candidat { get; set; }
        public DAO? DAO { get; set; }
        public MembreCommission? MembreCommission { get; set; }
    }

}