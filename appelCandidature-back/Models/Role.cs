using System.Text.Json.Serialization;

namespace pfe_back.Models
{
    public class Role
    {
        public int Id { get; set; }
        public required string Nom { get; set; }
        [JsonIgnore]
        public ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}
