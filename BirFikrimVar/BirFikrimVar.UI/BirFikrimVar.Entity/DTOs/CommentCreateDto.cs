using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirFikrimVar.Entity.DTOs
{
    public class CommentCreateDto
    {
        [Required(ErrorMessage =" Yorum Satırı Boş Olamaz")]
        public string? Content { get; set; }
        public int UserId { get; set; }
        public int IdeaId { get; set; }


    }
}
