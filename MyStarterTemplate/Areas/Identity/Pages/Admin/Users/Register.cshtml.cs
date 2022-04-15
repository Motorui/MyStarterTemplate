using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyStarterTemplate.Areas.Identity.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Users;

public class RegisterModel : BasePageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public RegisterModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();
    [BindProperty]
    public IList<ManageUserRolesViewModel> UserRoles { get; set; } = new List<ManageUserRolesViewModel>();

    public void OnGet()
    {

        foreach (var role in _roleManager.Roles.ToList())
        {
            var userRole = new ManageUserRolesViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Selected = false
            };
            UserRoles.Add(userRole);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {

        if (ModelState.IsValid)
        {
            var identity = new IdentityUser { UserName = Input.Username, Email = Input.Email };
            var result = await _userManager.CreateAsync(identity, Input.Password);

            var addUserRoleResult = await _userManager.AddToRolesAsync(identity, UserRoles.Where(x => x.Selected).Select(y => y.RoleName));

            if (result.Succeeded && addUserRoleResult.Succeeded)
            {
                // valores possíveis: primary, secondary, success, danger, warning, info,
                // light, dark, body, white, transparent
                ToastType = "success";
                ToastMessage = $"Utilizador {identity.UserName} criado com sucesso!";
                return LocalRedirect("~/Identity/Admin/Users");
            }
        }

        return Page();
    }

    public class InputModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
