using AutoMapper;
using BirFikrimVar.Business.Services;
using BirFikrimVar.Entity.DTOs;
using BirFikrimVar.Entity.Models;
using BirFikrimVar.Entity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;


namespace BirFikrimVar.UI.Controllers
{
    [Authorize]
    public class IdeaController : Controller
    {
        private readonly IIdeaService _ideaService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;
        private readonly IMapper _mapper;

        public IdeaController(IIdeaService ideaService, ICommentService commentService,ILikeService likeService, IMapper mapper)
        {
            _ideaService = ideaService;
            _commentService = commentService;
            _likeService = likeService;
            _mapper = mapper ;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> Create(IdeaCreateDto model)
        {
            
            if (!ModelState.IsValid) return View(model);

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            var idea = _mapper.Map<Idea>(model);
            idea.UserId = int.Parse(userIdClaim.Value);
            idea.CreatedAt = DateTime.Now;


            await _ideaService.AddIdeaAsync(idea);
            return RedirectToAction("Index", "Idea");
        }

        public async Task<IActionResult> Index(string? q, string sort = "new", int page = 1, int pageSize = 10)
        {
            // 1) verileri çek
            var ideas = await _ideaService.GetAllIdeasAsync();

            // 2) like bilgilerini hazırlamak için current user
            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                ideas = ideas.Where(i =>
                    (i.Title != null && i.Title.ToLower().Contains(lower)) ||
                    (i.Content != null && i.Content.ToLower().Contains(lower)) ||
                    (i.User != null && i.User.FullName != null && i.User.FullName.ToLower().Contains(lower))
                ).ToList();
            }

            //3 sıralama
            sort = string.IsNullOrWhiteSpace(sort) ? "new" : sort.ToLowerInvariant();
            var likeCountsTmp = new Dictionary<int, int>(); // liked sıralama için temin edebiliriz
            IEnumerable<BirFikrimVar.Entity.Models.Idea> ordered = ideas;

            switch (sort)
            {
                case "old":
                    ordered = ideas.OrderBy(i => i.CreatedAt);
                    break;

                case "liked":
                    // liked'a göre sıralayacağımız için likeCounts
                    foreach (var i in ideas)
                        likeCountsTmp[i.Id] = await _likeService.GetLikeCountAsync(i.Id);

                    ordered = ideas
                        .OrderByDescending(i => likeCountsTmp[i.Id])
                        .ThenByDescending(i => i.CreatedAt);
                    break;

                case "new":
                default:
                    ordered = ideas.OrderByDescending(i => i.CreatedAt);
                    break;
            }

            // 4) sayfalama
            var totalCount = ordered.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (page < 1) page = 1;
            if (totalPages > 0 && page > totalPages) page = totalPages;

            var paged = ordered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // 5) beğeni bilgileri (sadece sayfadaki kartlar için)
            int userId = 0;
            if (User.Identity.IsAuthenticated)
                userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            var likedMap = new Dictionary<int, bool>();
            var likeCounts = new Dictionary<int, int>();

            foreach (var idea in paged)
            {
                likeCounts[idea.Id] = await _likeService.GetLikeCountAsync(idea.Id);
                likedMap[idea.Id] = userId > 0
                    ? await _likeService.HasUserLikedAsync(idea.Id, userId)
                    : false;
            }

            // 6) viewbag'ler
            ViewBag.LikedMap = likedMap;
            ViewBag.LikeCounts = likeCounts;

            ViewBag.Query = q;
            ViewBag.Sort = sort;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            return View(paged);
        }


        [Authorize]
        public async Task<IActionResult> MyIdeas()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);
            var ideas = await _ideaService.GetIdeasByUserIdAsync(userId);
            ideas = ideas.OrderByDescending(i => i.CreatedAt).ToList();

            // boşsa ViewBag ile mesaj 
            if (ideas == null || ideas.Count == 0)
            {
                ViewBag.Message = "Henüz hiç fikir paylaşmadınız.";
            }

            var likedMap2 = new Dictionary<int, bool>();
            var likeCounts2 = new Dictionary<int, int>();

            foreach (var i in ideas)
            {
                likeCounts2[i.Id] = await _likeService.GetLikeCountAsync(i.Id);
                likedMap2[i.Id] = await _likeService.HasUserLikedAsync(i.Id, userId);
            }

            ViewBag.LikedMap = likedMap2;
            ViewBag.LikeCounts = likeCounts2;

            return View(ideas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var idea = await _ideaService.GetByIdWithUserAsync(id);
            if (idea == null) return NotFound();

            var comments = await _commentService.GetCommentsByIdeaIdAsync(id);

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            int userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;


            // Sahip mi?
            bool isOwner = userId > 0 && idea.UserId == userId;

            // 30 dk içinde mi? (CreatedAt set ediliyor)
            bool canEdit = isOwner && (DateTime.Now - idea.CreatedAt) <= TimeSpan.FromMinutes(30);


            var likeCount = await _likeService.GetLikeCountAsync(id);
            var hasLiked = userId > 0 && await _likeService.HasUserLikedAsync(id, userId);

            var viewModel = new CommentViewModel
            {
                IdeaId = id,
                IdeaTitle = idea.Title,
                IdeaContent = idea.Content,
                IdeaAuthorName = idea.User?.FullName,
                IdeaCreatedAt= idea.CreatedAt,

                Comments = comments,
                LikeCount = await _likeService.GetLikeCountAsync(id),
                HasLiked = userId > 0 && await _likeService.HasUserLikedAsync(id, userId),
                IsOwner = (userId > 0 && idea.UserId == userId),
                CanEdit = (userId > 0 && idea.UserId == userId && (DateTime.Now - idea.CreatedAt) <= TimeSpan.FromMinutes(30)),
                NewComment = new CommentCreateDto
                {
                    IdeaId = id,
                    UserId = userId
                },
                CommentLikeCounts = new Dictionary<int, int>(),
                UserLikedCommentIds = new HashSet<int>()
                

            };
            foreach (var c in comments)
            {
                var count = await _commentService.GetLikeCountAsync(c.Id);
                viewModel.CommentLikeCounts[c.Id] = count;

                if (userId > 0 && await _commentService.HasUserLikedAsync(c.Id, userId))
                    viewModel.UserLikedCommentIds.Add(c.Id);
            }

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var idea = await _ideaService.GetByIdAsync(id);
            if (idea == null) return NotFound();

            // sadece sahibi düzenleyebilsin
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || idea.UserId != int.Parse(userIdClaim.Value))
                return Forbid();

            var now = DateTime.Now;
            if ((now - idea.CreatedAt) > TimeSpan.FromMinutes(30))
            {
                TempData["Error"] = "Düzenleme süresi doldu (30 dakika geçti).";
                return RedirectToAction("Details", new { id = idea.Id });
            }


            var dto = new IdeaEditDto
            {
                Id = idea.Id,
                Title = idea.Title,
                Content = idea.Content
            };
            return View(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(IdeaEditDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var idea = await _ideaService.GetByIdAsync(model.Id);
            if (idea == null) return NotFound();

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || idea.UserId != int.Parse(userIdClaim.Value))
                return Forbid();

            var now = DateTime.Now;
            if ((now - idea.CreatedAt) > TimeSpan.FromMinutes(30))
            {
                TempData["Error"] = "Düzenleme süresi doldu (30 dakika geçti).";
                return RedirectToAction("Details", new { id = idea.Id });
            }


            // mevcut entity’ye dto’yu uygula (AutoMapper overload)
            _mapper.Map(model, idea);

            await _ideaService.UpdateIdeaAsync(idea);
            return RedirectToAction("Details", new { id = idea.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var idea = await _ideaService.GetByIdAsync(id);
            if (idea == null) return NotFound();

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || idea.UserId != int.Parse(userIdClaim.Value))
                return Forbid();

            await _ideaService.DeleteIdeaAsync(id);
            TempData["Info"] = "Fikir silindi.";
            return RedirectToAction("MyIdeas");
        }

    }
}
