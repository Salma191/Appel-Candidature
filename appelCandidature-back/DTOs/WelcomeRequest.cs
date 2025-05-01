namespace pfe_back.DTOs
{
    public class WelcomeRequest
    {
        public string? ToEmail { get; set; }
        public string? UserName { get; set; }
        public string? VerificationUrl { get; set; }
    }
}
