using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Entity.ViewModels
{
    public class CommentItemViewModel
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserFullName { get; set; }

        // like bilgileri
        public int LikeCount { get; set; }
        public bool HasLiked { get; set; }
    }
}
