using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Business.Services;
using BirFikrimVar.DataAccess.Repositories;
using BirFikrimVar.Entity.Models;

namespace BirFikrimVar.Business.Managers
{
    public class IdeaManager : IIdeaService
    {

        private readonly IGenericRepository<Idea> _idearepo;
        private readonly IGenericRepository<Comment> _commentRepo;
        private readonly IGenericRepository<Like> _likeRepo;

        public IdeaManager(IGenericRepository<Idea> idearepo, IGenericRepository<Comment> commentRepo,IGenericRepository<Like> likeRepo)
        {
            _idearepo = idearepo;
            _commentRepo = commentRepo;
            _likeRepo = likeRepo;
        }

        public async Task<List<Idea>> GetAllIdeasAsync() { 
            return await _idearepo.GetAllIncludingAsync(i => i.User);
        }

        public async Task AddIdeaAsync(Idea idea)
        {
            await _idearepo.AddAsync(idea);
        }

        public async Task<List<Idea>> GetIdeasByUserIdAsync(int userId) {
            return await _idearepo.FindAsync(i => i.UserId == userId);
        }

        public async Task<Idea> GetByIdWithUserAsync(int id)
        {
            var result = await _idearepo.GetAllAsync(i => i.Id == id, "User");
            return result.FirstOrDefault();
        }

        public async Task<Idea?> GetByIdAsync(int id)
        {
            return await _idearepo.GetByIdAsync(id);
        }

        public async Task UpdateIdeaAsync(Idea idea)
        {
            _idearepo.Update(idea);
            await _idearepo.SaveChangesAsync();
        }

        public async Task DeleteIdeaAsync(int id)
        {
            var idea = await _idearepo.GetByIdAsync(id);
            if (idea == null) return;

            // Beğenileri sil
            var likes = await _likeRepo.FindAsync(i => i.IdeaId == id);
            foreach (var like in likes)
            {
                _likeRepo.Delete(like);
            }

            // Yorumları sil
            var comments = await _commentRepo.FindAsync(c => c.IdeaId == id);
            foreach (var c in comments)
            {
                _commentRepo.Delete(c);
            }

            // Fikri sil
            _idearepo.Delete(idea);

            await _idearepo.SaveChangesAsync();
        }




    }
}
