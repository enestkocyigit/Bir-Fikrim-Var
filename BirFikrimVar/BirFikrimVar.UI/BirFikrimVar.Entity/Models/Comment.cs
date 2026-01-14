using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Entity.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Content { get; set; } 
        public DateTime CreatedAt { get; set; }

        public int IdeaId { get; set; }
        public Idea? Idea { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();

    }
}
