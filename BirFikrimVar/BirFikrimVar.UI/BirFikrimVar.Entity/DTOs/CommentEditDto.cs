using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Entity.DTOs
{
    public class CommentEditDto
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
