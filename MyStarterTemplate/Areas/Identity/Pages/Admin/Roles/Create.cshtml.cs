using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Roles;

public class CreateModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public CreateModel( RoleManager<IdentityRole> roleManager)
    { 
        _roleManager = roleManager;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();
    public string ReturnUrl { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var role = new IdentityRole(Input.Role);
            var addRoleResult = await _roleManager.CreateAsync(role);

            if (addRoleResult.Succeeded)
            {
                return RedirectToPage("./Index");
            }           
        }
        return Page();
    }

    public class InputModel
    {
        public string Role { get; set; } = string.Empty;
    }
}