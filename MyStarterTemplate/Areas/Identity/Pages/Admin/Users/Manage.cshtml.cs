using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStarterTemplate.Areas.Identity.ViewModels;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Users;

public class ManageModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ManageModel(UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public string ErrorMessage { get; set; } = string.Empty;
    public IdentityUser CurrentUser { get; set; } = new IdentityUser();
    [BindProperty]
    public IList<ManageUserRolesViewModel> UserRoles { get; set; } = new List<ManageUserRolesViewModel>();
    public string ReturnUrl { get; set; } = string.Empty;

    public async Task OnGetAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            ErrorMessage = $"User with Id = {id} cannot be found";
            return;
        }
        CurrentUser.UserName = user.UserName;

        foreach (var role in _roleManager.Roles.ToList())
        {
            var userRole = new ManageUserRolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                userRole.Selected = true;
            }
            else
            {
                userRole.Selected = false;
            }
            UserRoles.Add(userRole);
        }
    }
    public async Task<IActionResult> OnPostAsync(string id)
    {
        ReturnUrl = Url.Content($"~/Identity/Admin/Users/Manage?id={id}");
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Page();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return Page();
            }
            result = await _userManager.AddToRolesAsync(user, UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return Page();
            }
        }
        return LocalRedirect(ReturnUrl);
    }
}
