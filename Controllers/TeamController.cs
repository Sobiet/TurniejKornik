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
        return View(_context.Teams.Include(x => x.Participants).FirstOrDefault(x => x.Id == id));
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
    
    private bool TryLogin(string nickname, string password)
    {
        var participant = _context.Participants.SingleOrDefault(p => p.Nickname == nickname);

        if (participant == null) return false;

        if (participant.PasswordHash != AuthenticationHelper.GenerateHash(password, participant.Salt))
            return false;

        return true;
    }
}