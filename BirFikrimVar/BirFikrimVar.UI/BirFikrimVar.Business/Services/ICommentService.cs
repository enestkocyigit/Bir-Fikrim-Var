using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Entity.Models;

namespace BirFikrimVar.Business.Services
{
    public interface ICommentService
    {
        Task AddCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsByIdeaIdAsync(int ideaId);
        Task<Comment?> GetByIdAsync(int id);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id);

        // Likes
        Task<int> GetLikeCountAsync(int commentId);
        Task<bool> HasUserLikedAsync(int commentId, int userId);
        Task AddLikeAsync(int commentId, int userId);
        Task RemoveLikeAsync(int commentId, int userId);
        //Yorum sayfası
        Task<List<Comment>> GetCommentsByUserIdAsync(int userId);
    }
}
