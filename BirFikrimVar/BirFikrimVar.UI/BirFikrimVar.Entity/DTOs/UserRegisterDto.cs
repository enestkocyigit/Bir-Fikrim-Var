using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BirFikrimVar.Entity.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
