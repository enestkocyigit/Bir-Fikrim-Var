using BirFikrimVar.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class LikeController : Controller
{
    private readonly ILikeService _likeService;

    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int ideaId)
    {
       
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return RedirectToAction("Login", "Account");

        int userId = int.Parse(userIdClaim.Value);

        if (await _likeService.HasUserLikedAsync(ideaId, userId))
            await _likeService.RemoveLikeAsync(ideaId, userId);
        else
            await _likeService.AddLikeAsync(ideaId, userId);

        var referer = Request.Headers.Referer.ToString();
        if (!string.IsNullOrWhiteSpace(referer) &&
            Uri.TryCreate(referer, UriKind.Absolute, out var uri) &&
            uri.Host.Equals(Request.Host.Host, StringComparison.OrdinalIgnoreCase) &&
            uri.Port == Request.Host.Port)
        {
            return LocalRedirect(uri.PathAndQuery); // aynı sayfaya dön
        }

        // referer yoksa listeye dön
        return RedirectToAction("Index", "Idea");
    }
}




