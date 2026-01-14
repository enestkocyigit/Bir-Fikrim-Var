using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BirFikrimVar.Entity.DTOs
{
    public class IdeaCreateDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int UserId { get; set; }


    }
}
