namespace pfe_back.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTime DateEnvoi { get; set; }
        public int CandidatureId { get; set; }
        public Candidature Candidature { get; set; }
    }
}
