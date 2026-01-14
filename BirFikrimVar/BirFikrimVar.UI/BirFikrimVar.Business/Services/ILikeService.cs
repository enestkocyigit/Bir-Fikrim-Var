using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Business.Services;
using BirFikrimVar.DataAccess.Repositories;
using BirFikrimVar.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace BirFikrimVar.Business.Services
{
    public interface ILikeService
    {
        Task<int> GetLikeCountAsync(int ideaId);
        Task<bool> HasUserLikedAsync(int ideaId, int userId);
        Task AddLikeAsync(int IdeaId,int UserId);
        Task RemoveLikeAsync(int ideaId, int userId);
    }
}
