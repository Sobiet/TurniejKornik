using KornikTournament.Enums;

namespace KornikTournament.Models;

public class AddParticipant
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Nickname { get; set; }
    public ERole Role { get; set; }
    public required string Class { get; set; }
    public Guid TeamId { get; set; }
}