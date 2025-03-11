using pfe_back.Models;
using System.Text.Json.Serialization;

public class CreateCommissionDto
{
    public string? Nom { get; set; }
    public List<MembreCommissionDto>? MembreCommissions { get; set; }
}

public class MembreCommissionDto
{
    public int Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RoleCommission RoleCommission { get; set; }
}