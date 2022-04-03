using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStarterTemplate.Data;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Roles;
public class DeleteModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public DeleteModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Role = await _roleManager.FindByIdAsync(id);

        if (Role != null)
        {
            var removeRoleResult = await _roleManager.DeleteAsync(Role);

            if (removeRoleResult.Succeeded)
            {
                return RedirectToPage("./Index");
            }
        }

        return Page();
    }
}
