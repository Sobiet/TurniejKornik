using System.ComponentModel.DataAnnotations;

namespace KornikTournament.Models;

public class Team
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(4)]
    public required string Tag { get; set; }

    public List<Participant> Participants { get; set; } = null!;
}