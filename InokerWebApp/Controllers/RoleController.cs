using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InokerWebApp.Data;
using InokerWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InokerWebApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> rm;
        private readonly UserManager<ApplicationUser> um;
        private ApplicationDbContext db;

        public RoleController(RoleManager<IdentityRole> _rm, UserManager<ApplicationUser> _um, ApplicationDbContext _db)
        {
            rm = _rm;
            um = _um;
            db = _db;
        }

        [Authorize(Roles = "boss, admin")]
        public IActionResult Index(string message)
        {
            ViewBag.Message = message;
            List<IdentityRole> roles = rm.Roles.ToList();
            return View(roles);
        }

        [Authorize(Roles = "boss, admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "boss, admin")]
        public async Task<IActionResult> Create(IdentityRole Role)
        {
            var res = await rm.CreateAsync(Role);
            if (res.Succeeded)
            {
                return RedirectToAction("Index", new { message = $"Role <b>{Role.Name}</b> created" });
            }
            else
            {
                return View(Role);
            }
        }

        public async Task<IActionResult> Delete(string roleName)
        {
            IdentityRole role = await rm.FindByNameAsync(roleName);
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(string roleName)
        {
            IdentityRole rola = await rm.FindByNameAsync(roleName);
            db.Roles.Remove(rola);
            db.SaveChanges();
            return RedirectToAction("Index", new { message = $"Deleted role <strong>{roleName}</strong>" });

        }

        [Authorize(Roles = "boss, manager")]
        public IActionResult AddUserToRole(string message)
        {
            ViewBag.Message = message;
            ViewBag.Users = new SelectList(db.Users, "UserName", "UserName");
            FillRoleViewBag();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "boss, admin")]
        public async Task<IActionResult> AddUserToRole(string username, string role)
        {
            ApplicationUser user = await um.FindByNameAsync(username);
            if(user == null)
            {
                return RedirectToAction(nameof(AddUserToRole), new { message = $"Korisnik nije u bazi" });

            }
            bool result = await um.IsInRoleAsync(user, role);

            if (result)
            {
                return RedirectToAction(nameof(AddUserToRole), new { message = $"User <strong>{username}</strong> is already in role: <strong>{role}</strong>" });
            }
            var result2 = await um.AddToRoleAsync(user, role);
            if (result2.Succeeded)
            {
                return RedirectToAction(nameof(Index), new { message = $"User <strong>{username}</strong> is added to role: <strong>{role}</strong>" });
            }
            else
            {
                return RedirectToAction(nameof(AddUserToRole), new { message = $"Errors in adding the user: <br> " + result2.Errors.Select(e => e.Description + "<br>") });
            }
        }

        [Authorize(Roles = "boss, admin")]
        public IActionResult RemoveUserFromRole(string message)
        {
            ViewBag.Message = message;
            FillRoleViewBag();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "boss, admin")]
        public async Task<IActionResult> RemoveUserFromRole(string role, string username)
        {
            IdentityResult result = await um.RemoveFromRoleAsync(await um.FindByNameAsync(username), role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index), new { message = $"User <strong>{username}</strong> removed from role <strong>{role}</strong>" });
            }
            else
            {
                return RedirectToAction(nameof(RemoveUserFromRole), new { message = "Errors in removing user: <br> " + result.Errors.Select(e => e.Description + "<br>") });
            }
        }

        public async Task<PartialViewResult> _GetUsersFromRole(string role)
        {
            IList<ApplicationUser> users = await um.GetUsersInRoleAsync(role);
            return PartialView(users.ToList());
        }

        // Fills selectList viewbag with roles this user can interact with
        public void FillRoleViewBag()
        {
            if (User.IsInRole("admin") || User.IsInRole("boss"))
            {
                ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");
            }
            else if (User.IsInRole("Manager"))
            {
                ViewBag.Roles = new SelectList(db.Roles.Where(r => r.Name != "Admin" && r.Name != "Manager"), "Name", "Name");
            }
}
    }
}