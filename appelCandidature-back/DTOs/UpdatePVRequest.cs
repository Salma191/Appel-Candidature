namespace pfe_back.DTOs
{
    public class UpdatePVRequest
    {
        public PieceJointeRequest? PieceJointe { get; set; } // Changer string? en PieceJointeRequest?
    }

    public class PieceJointeRequest
    {
        public string? Nom { get; set; }
        public string? Fichier { get; set; }
        public string? Type { get; set; }
    }
}
