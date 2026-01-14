using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Entity.Models;

namespace BirFikrimVar.Business.Services
{
    public interface IIdeaService
    {
        Task<List<Idea>> GetAllIdeasAsync();
        Task AddIdeaAsync(Idea idea);
        Task<List<Idea>> GetIdeasByUserIdAsync(int userId);

        Task<Idea> GetByIdWithUserAsync(int id);
        Task<Idea?> GetByIdAsync(int id);
        Task UpdateIdeaAsync(Idea idea);

        Task DeleteIdeaAsync(int id);



    }
}
