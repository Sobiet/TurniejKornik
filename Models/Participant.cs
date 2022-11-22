using System.ComponentModel.DataAnnotations;
using KornikTournament.Enums;

namespace KornikTournament.Models;

public class Participant
{
    [Key]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Nickname { get; set; }
    public ERole Roles { get; set; }
    public required string Class { get; set; }
    public string? PasswordHash { get; set; }
    public string? Salt { get; set; }
    public bool Leader { get; set; }

    public Team Team { get; set; } = null!;
}