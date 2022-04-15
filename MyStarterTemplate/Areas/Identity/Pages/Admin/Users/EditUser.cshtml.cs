using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyStarterTemplate.Areas.Identity.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Users;

public class EditUserModel : BasePageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public EditUserModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();
    [BindProperty]
    public IList<ManageUserRolesViewModel> UserRoles { get; set; } = new List<ManageUserRolesViewModel>();

    public async Task OnGetAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            // valores possíveis: primary, secondary, success, danger, warning, info,
            // light, dark, body, white, transparent
            ToastType = "danger";
            ToastMessage = $"O utilizador com o Id = {id} não encontrado";
            return;
        }
        Input.Username = user.UserName;
        Input.Email = user.Email;
        Input.Password = user.PasswordHash;

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
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.UserName = Input.Username;
            user.Email = Input.Email;

            if (user == null)
            {
                return Page();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var updatePassword = await _userManager.ResetPasswordAsync(user, token, Input.Password);
            if (!updatePassword.Succeeded)
            {
                ModelState.AddModelError("", "Erro a alterar a password");
                return Page();
            }

            var updateUser = await _userManager.UpdateAsync(user);
            if (!updateUser.Succeeded)
            {
                ModelState.AddModelError("", "Erro a alterar o user");
                return Page();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Não é possível remover os roles existentes");
                return Page();
            }

            result = await _userManager.AddToRolesAsync(user, UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Não é possível adicionar os roles selecionados");
                return Page();
            }
        }
        // valores possíveis: primary, secondary, success, danger, warning, info,
        // light, dark, body, white, transparent
        ToastType = "success";
        ToastMessage = $"Utilizador atualizado com sucesso";
        return LocalRedirect("~/Identity/Admin/Users/Index");
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
