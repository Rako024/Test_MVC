using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class TeamController : Controller
    {
        ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public IActionResult Index()
        {
            List<Team> teams = _teamService.GetAllTeams();
            return View(teams);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _teamService.CreateTeam(team);
            }catch(TeamNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (NotFoundPhotoFileException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _teamService.DeleteTeam(id);
            }
            catch (TeamNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Team team = _teamService.GetTeam(x=>x.Id == id);
            if(team == null)
            {
                ModelState.AddModelError("", "Team is null!");
                return RedirectToAction(nameof(Index)); 
            }
            return View(team);
        }

        [HttpPost]
        public IActionResult Update(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _teamService.UpdateTeam(team.Id, team);
            }
            catch (TeamNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
