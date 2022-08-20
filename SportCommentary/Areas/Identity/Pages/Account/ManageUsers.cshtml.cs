using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportCommentary.Data;
using Microsoft.AspNetCore.Identity;

namespace SportCommentary.Areas.Identity.Pages.Account
{
    [Authorize(Policy = "PolicyAdmin")]
    public class ManageUsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ManageUsersModel> _logger;

        public List<UsersViewModel> AllUsers = new List<UsersViewModel>();
        public ManageUsersModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<ManageUsersModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public class UsersViewModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }
        public async Task OnGetAsync(string message)
        {
            ViewData["ManageMessage"] = message;
            AllUsers = _userManager.Users.Select(c => new UsersViewModel()
            {
                Username = c.UserName,
                Email = c.Email,
                Role = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray())
            }).ToList();

        }

        [HttpPost]
        public async Task <IActionResult> OnPostAsync(string user, string role)
        {
            IdentityResult result = new IdentityResult();
            try
            {
                if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(role))
                {
                    var userFromManager = await _userManager.FindByEmailAsync(user);
                    if (userFromManager != null)
                    {
                        if (User.Identity.Name.Equals(userFromManager.Email))
                        {
                            return RedirectToAction("OnGetAsync", new { message = "Nie mo¿na zmieniæ uprawnieñ sobie samemu!" });
                        }

                        switch (role)
                        {
                            case "giveAdmin":
                                if (!await _userManager.IsInRoleAsync(userFromManager, "Admin"))
                                {
                                    result = await _userManager.AddToRoleAsync(userFromManager, "Admin");
                                }
                                break;
                            case "undoAdmin":
                                if (await _userManager.IsInRoleAsync(userFromManager, "Admin"))
                                {
                                    result = await _userManager.RemoveFromRoleAsync(userFromManager, "Admin");
                                }
                                break;
                            case "giveCommentator":
                                if (!await _userManager.IsInRoleAsync(userFromManager, "Commentator"))
                                {
                                    result = await _userManager.AddToRoleAsync(userFromManager, "Commentator");
                                }
                                break;
                            case "undoCommentator":
                                {
                                    if (await _userManager.IsInRoleAsync(userFromManager, "Commentator"))
                                    {
                                        result = await _userManager.RemoveFromRoleAsync(userFromManager, "Commentator");
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (result.Succeeded)
                    {
                        return RedirectToAction("OnGetAsync");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OnGetSubmitUserChange]");
            }
            
            return Page();
        }
    }
}
