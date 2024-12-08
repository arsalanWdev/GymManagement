using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using GymManagement.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace RoleBase.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this._context = context;
        }

        // Action to list all users and their roles
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = string.Join(", ", roles)
                });
            }

            return View(userRoles);
        }
        // GET: Create User
        [HttpGet]
        public IActionResult Create()
        {
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new CreateUserViewModel
            {
                Roles = allRoles.Select(role => new RoleSelectionViewModel
                {
                    RoleName = role,
                    IsSelected = false
                }).ToList()
            };

            return View(model);
        }

        // POST: Create User
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = _roleManager.Roles.Select(r => new RoleSelectionViewModel
                {
                    RoleName = r.Name,
                    IsSelected = false
                }).ToList();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName);
                if (selectedRoles.Any())
                {
                    await _userManager.AddToRolesAsync(user, selectedRoles);
                }

                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.Roles = _roleManager.Roles.Select(r => new RoleSelectionViewModel
            {
                RoleName = r.Name,
                IsSelected = false
            }).ToList();

            return View(model);
        }



        // GET: Edit User Roles
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new EditUserRoleViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = allRoles.Select(role => new UserRoleAssignment
                {
                    RoleName = role,
                    IsAssigned = userRoles.Contains(role)
                }).ToList()
            };

            return View(model);
        }

        // POST: Edit User Roles
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.Roles.Where(r => r.IsAssigned && !currentRoles.Contains(r.RoleName)).Select(r => r.RoleName);
            var rolesToRemove = currentRoles.Where(r => !model.Roles.Any(er => er.IsAssigned && er.RoleName == r));

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            TempData["SuccessMessage"] = "User Edited Successfully";
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete Confirmation
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(new UserRoleViewModel { Id = user.Id, Email = user.Email });
        }

        // POST: Confirm Delete
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            TempData["SuccessMessage"] = "User Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
       

    }





    // ViewModel to hold user data along with roles
    public class UserRoleViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }

    // ViewModel for Editing User Roles
    public class EditUserRoleViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<UserRoleAssignment> Roles { get; set; }
    }

    public class UserRoleAssignment
    {
        public string RoleName { get; set; }
        public bool IsAssigned { get; set; }
    }
    public class RoleSelectionViewModel
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
