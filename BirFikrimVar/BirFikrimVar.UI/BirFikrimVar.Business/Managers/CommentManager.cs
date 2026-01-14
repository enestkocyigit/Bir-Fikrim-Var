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
    public class CommentManager :  ICommentService 
    {
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<CommentLike> _commentLikeRepo;

        public CommentManager(IGenericRepository<Comment> commentRepository,IGenericRepository<CommentLike> commentLikeRepo)
        {
            _commentRepository = commentRepository;
            _commentLikeRepo = commentLikeRepo;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _commentRepository.AddAsync(comment);
        }

        public async Task<List<Comment>> GetCommentsByIdeaIdAsync(int ideaId)
        {
            return await _commentRepository.GetAllAsync(c => c.IdeaId == ideaId, includeProperties: "User");
        }

        public async Task<Comment?> GetByIdAsync(int id)
        => await _commentRepository.GetByIdAsync(id);

        public async Task UpdateAsync(Comment comment)
        {
            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var c = await _commentRepository.GetByIdAsync(id);
            if (c == null) return;

            // önce like’ları temizle
            var likes = await _commentLikeRepo.FindAsync(x => x.CommentId == id);
            foreach (var l in likes) _commentLikeRepo.Delete(l);

            _commentRepository.Delete(c);
            await _commentRepository.SaveChangesAsync();
        }

        // --- Likes ---
        public async Task<int> GetLikeCountAsync(int commentId)
        {
            var list = await _commentLikeRepo.FindAsync(x => x.CommentId == commentId);
            return list.Count;
        }

        public async Task<bool> HasUserLikedAsync(int commentId, int userId)
        {
            var list = await _commentLikeRepo.FindAsync(x => x.CommentId == commentId && x.UserId == userId);
            return list.Any();
        }

        public async Task AddLikeAsync(int commentId, int userId)
        {
            // zaten beğenmişse ekleme
            if (await HasUserLikedAsync(commentId, userId)) return;
            await _commentLikeRepo.AddAsync(new CommentLike { CommentId = commentId, UserId = userId });
        }

        public async Task RemoveLikeAsync(int commentId, int userId)
        {
            var list = await _commentLikeRepo.FindAsync(x => x.CommentId == commentId && x.UserId == userId);
            foreach (var l in list) _commentLikeRepo.Delete(l);
            await _commentLikeRepo.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetCommentsByUserIdAsync(int userId)
        {
            var list = await _commentRepository.GetAllAsync(
                c => c.UserId == userId,
                includeProperties: "Idea,Idea.User"
            );
            return list.OrderByDescending(c => c.CreatedAt).ToList();
        }
    }
}
