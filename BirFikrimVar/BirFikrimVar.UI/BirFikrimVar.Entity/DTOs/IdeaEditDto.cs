using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Entity.DTOs
{
    public class IdeaEditDto
    {
        public int Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
    }
}
