namespace pfe_back.Models
{
    public class Organigramme
    {
        public int Id { get; set; }
        public string? Sigle { get; set; }
        public string? RattachementHierarchique { get; set; }
        public string? Poste { get; set; }
        public string? TypePoste { get; set; }
        public string? Localite { get; set; }
        public string? Titulaire { get; set; } 
        public DateOnly? DateNomination { get; set; }
    }

}
