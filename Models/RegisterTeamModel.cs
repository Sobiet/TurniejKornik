namespace KornikTournament.Models;

public class RegisterTeamModel
{
    public required string Name { get; set; }
    public required string Tag { get; set; }
    
    public required string LeaderName { get; set; }
    public required string LeaderSurname { get; set; }
    public required string LeaderNickname { get; set; }
    public required string LeaderClass { get; set; }
    public required string Role { get; set; }
    public string Password { get; set; } = null!;
}