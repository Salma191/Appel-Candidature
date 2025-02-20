using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class Utilisateur
    {
        [Key]
        public int Id { get; set; }

        public required string Nom { get; set; }
        public required string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        public required string Password { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }

}