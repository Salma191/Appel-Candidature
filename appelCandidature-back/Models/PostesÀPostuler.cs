namespace pfe_back.Models
{
    public class PostesÀPostuler
    {
        public int Id { get; set; }
        public int PosteId { get; set; }
        public int CandidatId { get; set; }
        public DateTime DateAjout { get; set; }
        public Poste? Poste { get; set; }
        public Candidat? Candidat { get; set; }
    }
}
