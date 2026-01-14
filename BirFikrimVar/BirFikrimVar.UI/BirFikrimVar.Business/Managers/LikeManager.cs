using BirFikrimVar.Business.Services;
using BirFikrimVar.DataAccess.Repositories;
using BirFikrimVar.Entity.Models;

public class LikeManager : ILikeService
{
    private readonly IGenericRepository<Like> _likeRepository;
    public LikeManager(IGenericRepository<Like> likeRepository)
    {
        _likeRepository = likeRepository;
    }

    public async Task<int> GetLikeCountAsync(int ideaId)
    {
        var likes = await _likeRepository.GetAllAsync(l => l.IdeaId == ideaId);
        return likes.Count;
    }

    public async Task<bool> HasUserLikedAsync(int ideaId, int userId)
    {
        var likes = await _likeRepository.GetAllAsync(l => l.IdeaId == ideaId && l.UserId == userId);
        return likes.Any();
    }

    public async Task AddLikeAsync(int ideaId, int userId)
    {
        await _likeRepository.AddAsync(new Like { IdeaId = ideaId, UserId = userId });
    }

    public async Task RemoveLikeAsync(int ideaId, int userId)
    {
        var likes = await _likeRepository.GetAllAsync(l => l.IdeaId == ideaId && l.UserId == userId);
        foreach (var like in likes)
            _likeRepository.Delete(like);

        await _likeRepository.SaveChangesAsync();
    }
}

