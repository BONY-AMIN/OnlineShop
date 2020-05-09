using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> userManager;
        ApplicationDbContext db;

        public UserController(UserManager<IdentityUser> _userManager,ApplicationDbContext _db)
        {
            userManager = _userManager;
            db = _db;
        }

        public IActionResult Index()
        {
            return View(db.ApplicationUsers.ToList());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            var result = await userManager.CreateAsync(user,user.PasswordHash);
            if (result.Succeeded)
            {
                TempData["Save"] = "User has been created successfully .";
                return RedirectToAction(nameof(Index));

            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
    }
}