using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyStarterTemplate.Areas.Identity.ViewModels;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Users;
public class IndexModel : BasePageModel
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
            var thisViewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = await GetUserRoles(user)
            };
            UsersWithRoles.Add(thisViewModel);
        }
    }

    private async Task<List<string>> GetUserRoles(IdentityUser user)
    {
        return new List<string>(await _userManager.GetRolesAsync(user));
    }
}
