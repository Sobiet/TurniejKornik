using KornikTournament.Data;
using KornikTournament.Helpers;
using KornikTournament.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KornikTournament.Controllers;

[Route("[controller]")]
public class TeamController : Controller
{
    private readonly ApplicationContext _context;

    public TeamController(ApplicationContext context)
    {
        _context = context;
    }

    public IActionResult Index([FromQuery] Guid id)
    {
        ViewData["Team"] = _context.Teams.Include(x => x.Participants).FirstOrDefault(x => x.Id == id);
        return View();
    }
    
    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public IActionResult LoginConfirmed(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var participant = _context.Participants.Include(x => x.Team).FirstOrDefault(x => x.Nickname == loginModel.Nickname);

            if (participant is null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (!TryLogin(loginModel.Nickname, loginModel.Password))
            {
                return RedirectToAction(nameof(Login));
            }
            
            if (participant.Leader)
            {
                return RedirectToAction(nameof(Index), "Team", new {Id = participant.Team.Id});
            }
        }

        return RedirectToAction(nameof(Login));
    }
    
    [HttpPost("Add")]
    [ValidateAntiForgeryToken]
    public IActionResult AddToTeam(AddParticipant participant)
    {
        var team = _context.Teams.Include(x => x.Participants).FirstOrDefault(x => x.Id == participant.TeamId);

        if (team!.Participants.Count >= 5)
        {
            return RedirectToAction(nameof(Index), new {id = participant.TeamId});
        }
        
        _context.Participants.Add(new Participant
        {
            Id = Guid.NewGuid(),
            Name = participant.Name,
            Surname = participant.Surname,
            Nickname = participant.Nickname,
            Roles = participant.Role,
            Class = participant.Class,
            Team = team
        });

        _context.SaveChanges();
        
        return RedirectToAction(nameof(Index), new {id = participant.TeamId});
    }
    
    private bool TryLogin(string nickname, string password)
    {
        var participant = _context.Participants.SingleOrDefault(p => p.Nickname == nickname);

        if (participant == null) return false;

        if (participant.PasswordHash != AuthenticationHelper.GenerateHash(password, participant.Salt))
            return false;

        return true;
    }
}