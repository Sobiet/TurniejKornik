using System.Diagnostics;
using KornikTournament.Data;
using KornikTournament.Enums;
using KornikTournament.Helpers;
using Microsoft.AspNetCore.Mvc;
using KornikTournament.Models;

namespace KornikTournament.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateTeam(RegisterTeamModel model)
    {
        if (ModelState.IsValid)
        {
            var any = _context.Teams.Any(x => x.Tag == model.Tag);

            if (any)
            {
                return RedirectToAction(nameof(Index));
            }
            
            var leader = new Participant
            {
                Id = Guid.NewGuid(),
                Name = model.LeaderName,
                Surname = model.LeaderSurname,
                Nickname = model.LeaderNickname,
                Class = model.LeaderClass,
                PasswordHash = model.Password,
                Leader = true,
                Roles = Enum.Parse<ERole>(model.Role)
            };

            _context.Teams.Add(new Team
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Tag = model.Tag,
                Participants = new List<Participant> {leader}
            });
            
            var success = Register(model.LeaderNickname, leader);

            if (!success.success)
            {
                return RedirectToAction(nameof(Index));
            }
            
            _context.SaveChanges();

            return RedirectToAction("Login", "Team");
        }
        
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    private (bool success, string content) Register(string nickname, Participant participant)
    {
        if (_context.Participants.Any(x => x.Nickname == nickname))
            return (false, "Username not available");
        
        participant.ProvideSaltAndHash();

        _context.Add(participant);
        _context.SaveChanges();

        return (true, string.Empty);
    }
}