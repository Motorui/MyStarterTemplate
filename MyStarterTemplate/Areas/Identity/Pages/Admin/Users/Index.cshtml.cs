using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStarterTemplate.Areas.Identity.ViewModels;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Users;
public class IndexModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public IndexModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public IList<UserRolesViewModel> UsersWithRoles { get; set; } = new List<UserRolesViewModel>();

    public async Task OnGetAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        foreach (IdentityUser user in users)
        {
            var thisViewModel = new UserRolesViewModel();
            thisViewModel.UserId = user.Id;
            thisViewModel.Email = user.Email;
            thisViewModel.UserName = user.UserName;
            thisViewModel.Roles = await GetUserRoles(user);
            UsersWithRoles.Add(thisViewModel);
        }
    }

    private async Task<List<string>> GetUserRoles(IdentityUser user)
    {
        return new List<string>(await _userManager.GetRolesAsync(user));
    }
}
