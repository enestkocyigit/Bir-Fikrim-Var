using System;
using System.Collections.Generic;

namespace BirFikrimVar.Entity.ViewModels
{
    public class MyCommentItemVm
    {
        public int CommentId { get; set; }
        public int IdeaId { get; set; }
        public string IdeaTitle { get; set; } = "";
        public string? IdeaAuthorName { get; set; }
        public string CommentContent { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
    }

    public class MyCommentsViewModel
    {
        public List<MyCommentItemVm> Items { get; set; } = new();
    }
}

