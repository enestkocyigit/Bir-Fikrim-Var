using AutoMapper;
using BirFikrimVar.Business.Services;
using BirFikrimVar.Entity.DTOs;
using BirFikrimVar.Entity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BirFikrimVar.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register() { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto model) {



            if (!ModelState.IsValid) return View(model);

            var user = _mapper.Map<User>(model);
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            await _userService.AddUserAsync(user);
            return RedirectToAction("Login", "Account");


        }


        [HttpGet]
        public IActionResult Login() {
            
            return View("~/Views/Account/Login.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (!ModelState.IsValid) return View("~/Views/Account/Login.cshtml", model);

            // 1) E-mail’e göre kullanıcıyı çek
            var user = await _userService.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
                return View(model);
            }

            // 2) Hash doğrulaması
            var ok = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
                return View(model);
            }

            // 3) Session + Cookie ile giriş yap
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserFullName", user.FullName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Idea");
        }

        [HttpPost]
        public async Task<IActionResult> Logout() { 
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
