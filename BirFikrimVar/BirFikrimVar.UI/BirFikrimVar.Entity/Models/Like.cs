using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Entity.Models;

namespace BirFikrimVar.Entity.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int IdeaId { get; set; }
        public int UserId { get; set; }

        public Idea? Idea { get; set; }
        public User? User { get; set; }
    }
}
