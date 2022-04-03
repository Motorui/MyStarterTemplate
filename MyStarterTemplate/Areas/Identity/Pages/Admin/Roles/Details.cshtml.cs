using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Roles;

public class DetailsModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public DetailsModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IdentityRole Role { get; set; } = new IdentityRole();

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Role = await _roleManager.FindByIdAsync(id);

        if (Role == null)
        {
            return NotFound();
        }
        return Page();
    }
}
