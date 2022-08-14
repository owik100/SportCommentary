using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportCommentary.Data;
using SportCommentaryDataAccess.EFModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SportCommentary.Areas.Identity.Pages.Account
{
    [Authorize(Policy = "PolicyAdmin")]
    public class ManageUsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public List<UsersViewModel> AllUsers = new List<UsersViewModel>();
        public ManageUsersModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public class UsersViewModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }
        public async Task OnGetAsync()
        {
            AllUsers = _userManager.Users.Select(c => new UsersViewModel()
            {
                Username = c.UserName,
                Email = c.Email,
                Role = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray())
            }).ToList();

        }
    }
}
