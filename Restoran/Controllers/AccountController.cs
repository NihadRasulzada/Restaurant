using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restoran.DTOs;
using Restoran.Enums;
using Restoran.Models.Account;

namespace Softy_Pinko.Controllers
{
    public class AccountController : Controller
    {
        #region CTOR
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            AppUser? appUser = await _userManager.FindByNameAsync(loginDto.UserName);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(appUser, loginDto.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                return View();
            }
            if (!signInResult.Succeeded)
            {
                return View();
            }
            await _signInManager.SignInAsync(appUser, true);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region SignUp
        public IActionResult SignUp()
        {
            ViewBag.Role = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.User.ToString(),
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterDto registerDto, string role)
        {
            var validRoles = new List<string>
    {
        Roles.Admin.ToString(),
        Roles.User.ToString(),
    };

            if (string.IsNullOrEmpty(role) || !validRoles.Contains(role))
            {
                ModelState.AddModelError("Role", "The selected role is not valid.");
                ViewBag.Role = validRoles;
                return View(registerDto);
            }

            AppUser appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };


            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewBag.Role = validRoles;
                return View(registerDto);
            }

            var createdUser = await _userManager.FindByEmailAsync(registerDto.Email);
            var addToRoleResult = await _userManager.AddToRoleAsync(createdUser, role);

            if (!addToRoleResult.Succeeded)
            {
                foreach (var error in addToRoleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewBag.Role = validRoles;
                return View(registerDto);
            }

            await _signInManager.SignInAsync(createdUser, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region CreateRole
        public async Task CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new AppRole { Name = Roles.Admin.ToString(), Id = Guid.NewGuid().ToString() });
            }
            if (!await _roleManager.RoleExistsAsync(Roles.User.ToString()))
            {
                await _roleManager.CreateAsync(new AppRole { Name = Roles.User.ToString(), Id = Guid.NewGuid().ToString() });
            }
        }
        #endregion
    }
}
