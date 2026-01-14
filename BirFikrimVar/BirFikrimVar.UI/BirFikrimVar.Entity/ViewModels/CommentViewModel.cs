using BirFikrimVar.Entity.Models;
using BirFikrimVar.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Entity.ViewModels
{
    public class CommentViewModel
    {
        public int IdeaId { get; set; }
        public string? IdeaTitle { get; set; }
        public string? IdeaContent { get; set; }
        public string? IdeaAuthorName { get; set; }
        public int LikeCount { get; set; } //fikir beğeni sayısı
        public bool HasLiked { get; set; }//fikri beğendim mi
        public DateTime IdeaCreatedAt { get; set; }

        public List<Comment>?  Comments { get; set; }
        public List<CommentItemViewModel> CommentItems { get; set; } = new();
        public CommentCreateDto? NewComment { get; set; }

        public bool IsOwner { get; set; }
        public bool CanEdit { get; set; }

        public Dictionary<int, int> CommentLikeCounts { get; set; } = new();
        public HashSet<int> UserLikedCommentIds { get; set; } = new();

    }
}
