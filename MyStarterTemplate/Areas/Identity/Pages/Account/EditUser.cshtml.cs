using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MyStarterTemplate.Areas.Identity.Pages.Account;

public class EditUserModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    public EditUserModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();
    public string ReturnUrl { get; set; } = string.Empty;

    public void OnGet()
    {
        ReturnUrl = Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ReturnUrl = Url.Content("~/");
        if (ModelState.IsValid)
        {
            var identity = await _userManager.FindByNameAsync(User.Identity!.Name);

            identity.UserName = Input.Username;
            identity.Email = Input.Email;
            identity.PasswordHash = Input.Password;

            var result = await _userManager.UpdateAsync(identity);

            var claim = new Claim("uh", Input.UnidadeDeHandling.ToUpper());

            var claimsResult = await _userManager.AddClaimAsync(identity, claim);

            if (result.Succeeded && claimsResult.Succeeded)
            {
                await _signInManager.SignInAsync(identity, isPersistent: false);
                return LocalRedirect(ReturnUrl);
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
        [Required]
        public string UnidadeDeHandling { get; set; } = string.Empty;
    }
}
