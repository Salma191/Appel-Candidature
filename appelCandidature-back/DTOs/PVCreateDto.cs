using pfe_back.Models;

namespace pfe_back.DTOs
{
    public class PVCreateDto
    {
        public string? Reference { get; set; }
        public int TypePosteId { get; set; }
        public string? RattachementHierarchique { get; set; }
        public List<int> OrganigrammeIds { get; set; } = new();
        public Commission? Commission { get; set; }

    }
}
