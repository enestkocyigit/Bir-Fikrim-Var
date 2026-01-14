using BirFikrimVar.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Entity.ViewModels
{
    public class IdeaCardViewModel
    {
        public Idea Idea { get; set; } = null!;
        public bool CanEdit { get; set; }         // 30dk kuralı vb.
        public bool HasLiked { get; set; }
        public int LikeCount { get; set; }
        public bool ShowDelete { get; set; } = true;
        public bool ShowEdit { get; set; } = true; // CanEdit true ise gösterelim
    }
}
