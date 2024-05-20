using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAsbtracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes
{
    public class TeamService : ITeamService
    {
        ITeamRepository _repository;
        IWebHostEnvironment _webHostEnvironment;
        public TeamService(ITeamRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void CreateTeam(Team team)
        {
            if(team == null)
            {
                throw new TeamNullException("", "Team is null refereance!");
            }
            if(team.PhotoFile == null)
            {
                throw new NotFoundPhotoFileException("PhotoFile", "PhotoFile is null!");
            }
            if(!team.PhotoFile.ContentType.Contains("image/")) 
            {
                throw new ContentTypeException("PhotoFile", "Content Type is not Valid!");
            }
            string path = _webHostEnvironment.WebRootPath + @"\upload\team\"+ team.PhotoFile.FileName;
            using(FileStream file = new FileStream(path, FileMode.Create))
            {
                team.PhotoFile.CopyTo(file);
            }
            team.ImgUrl = team.PhotoFile.FileName;
            _repository.Add(team);
            _repository.Commit();

        }

        public void DeleteTeam(int id)
        {
            Team team = _repository.Get(x => x.Id == id);
            if(team == null)
            {
                throw new TeamNullException("", "Team is null!");
            }
            _repository.Delete(team);
            _repository.Commit();
        }

        public List<Team> GetAllTeams(Func<Team, bool>? func = null)
        {
            return _repository.GetAll(func);
        }

        public Team GetTeam(Func<Team, bool>? func = null)
        {
            return _repository.Get(func);
        }

        public void UpdateTeam(int id, Team newTeam)
        {
            Team oldTeam = _repository.Get(x => x.Id == id);
            if (oldTeam == null)
            {
                throw new TeamNullException("", "Team is null!");
            }

            if(newTeam.PhotoFile !=null)
            {
                string path1 = _webHostEnvironment.WebRootPath + @"\upload\team\" + oldTeam.ImgUrl;
                FileInfo fileInfo1 = new FileInfo(path1);
                fileInfo1.Delete();
                if (!newTeam.PhotoFile.ContentType.Contains("image/"))
                {
                    throw new ContentTypeException("PhotoFile", "Content Type is not Valid!");
                }
                string path = _webHostEnvironment.WebRootPath + @"\upload\team\" + newTeam.PhotoFile.FileName;
                using (FileStream file = new FileStream(path, FileMode.Create))
                {
                    newTeam.PhotoFile.CopyTo(file);
                }
                
                oldTeam.ImgUrl = newTeam.PhotoFile.FileName;
            }
            oldTeam.FullName = newTeam.FullName;
            oldTeam.Description = newTeam.Description;
            oldTeam.Position = newTeam.Position;
            _repository.Commit();
        }
    }
}
