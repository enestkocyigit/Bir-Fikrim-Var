using BirFikrimVar.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace BirFikrimVar.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) { 
            _userService = userService;
        }

        public async Task<IActionResult> Index() { 
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }
    }
}
