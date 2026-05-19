using Microsoft.AspNetCore.Mvc;
using ASSIGNTMENT.Models;

namespace ASSIGNTMENT.Controllers
{
    public class AccountController : Controller
    {
        private readonly FastFoodDbContext _context;

        public AccountController(FastFoodDbContext context)
        {
            _context = context;
        }

        // ===== LOGIN =====
        public IActionResult Login()
        {
            return View();
        }

        // ===== REGISTER =====
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetInt32("RoleId", (int)user.RoleId);

                // 👉 SỬA Ở ĐÂY (PHÂN TRANG SAU LOGIN)
                if (user.RoleId == 1)
                {
                    return RedirectToAction("Index", "Foods");   // ADMIN
                }
                else
                {
                    return RedirectToAction("Index", "Home");   // CLIENT
                }
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.RoleId = 2; // Customer

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(user);
        }
    }
}