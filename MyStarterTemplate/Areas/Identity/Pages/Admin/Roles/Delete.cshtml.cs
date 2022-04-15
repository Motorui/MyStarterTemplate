using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Roles;
public class DeleteModel : BasePageModel
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
                // valores possíveis: primary, secondary, success, danger, warning, info,
                // light, dark, body, white, transparent
                ToastType = "success";
                ToastMessage = "Role apagado com sucesso!";
                return RedirectToPage("./Index");
            }
        }

        return Page();
    }
}
