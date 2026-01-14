using AutoMapper;
using BirFikrimVar.Business.Services;
using BirFikrimVar.Entity.DTOs;
using BirFikrimVar.Entity.Models;
using BirFikrimVar.Entity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BirFikrimVar.UI.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix ="NewComment")] CommentCreateDto model)
        {
            

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return RedirectToAction("Details", "Idea", new { id = model.IdeaId });

            var comment = _mapper.Map<Comment>(model);
            comment.UserId = int.Parse(userIdClaim.Value);
            comment.CreatedAt = DateTime.Now;

            await _commentService.AddCommentAsync(comment);

            return RedirectToAction("Details", "Idea", new { id = model.IdeaId });
        }

        // YORUM DÜZENLE (FORMU GÖSTER)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var c = await _commentService.GetByIdAsync(id);
            if (c == null) return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (c.UserId != userId) return Forbid();

            var dto = new CommentEditDto
            {
                Id = c.Id,
                IdeaId = c.IdeaId,
                Content = c.Content
            };
            return View(dto); 
        }

        // YORUM DÜZENLE (POST)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CommentEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IdeaId = dto.IdeaId;
                return View(dto);
            }

            var c = await _commentService.GetByIdAsync(dto.Id);
            if (c == null) return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (c.UserId != userId) return Forbid();

            c.Content = dto.Content?.Trim();
            await _commentService.UpdateAsync(c);

            return RedirectToAction("Details", "Idea", new { id = dto.IdeaId });
        }

        // YORUM SİL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int ideaId, string? returnTo = "idea")
        {
            var c = await _commentService.GetByIdAsync(id);
            if (c == null) return NotFound();

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            if (c.UserId != userId) return Forbid();

            await _commentService.DeleteAsync(id);
            TempData["Info"] = "Yorum silindi.";

            return returnTo == "my"
                ? RedirectToAction("My", "Comment")
                : RedirectToAction("Details", "Idea", new { id = ideaId });
        }

        // YORUM BEĞEN / GERİ AL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(int commentId, int ideaId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (await _commentService.HasUserLikedAsync(commentId, userId))
                await _commentService.RemoveLikeAsync(commentId, userId);
            else
                await _commentService.AddLikeAsync(commentId, userId);

            // aynı detay sayfasında kal
            return RedirectToAction("Details", "Idea", new { id = ideaId });

            
        }

        public async Task<IActionResult> My()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return RedirectToAction("Login", "Account");
            int userId = int.Parse(userIdClaim.Value);

            var comments = await _commentService.GetCommentsByUserIdAsync(userId);

            var vm = new MyCommentsViewModel();
            foreach (var c in comments)
            {
                vm.Items.Add(new MyCommentItemVm
                {
                    CommentId = c.Id,
                    IdeaId = c.IdeaId,
                    IdeaTitle = c.Idea?.Title ?? "(Başlık yok)",
                    IdeaAuthorName = c.Idea?.User?.FullName,
                    CommentContent = c.Content ?? "",
                    CreatedAt = c.CreatedAt,
                    LikeCount = await _commentService.GetLikeCountAsync(c.Id)
                });

            }

            return View(vm);
        }
    }
}

