using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStarterTemplate.Data;

namespace MyStarterTemplate.Areas.Identity.Pages.Admin.Roles;

public class IndexModel : PageModel
{
    private readonly DataContext _context;

    public IndexModel(DataContext context)
    {
        _context = context;
    }

    public IList<IdentityRole> IdentityRoles { get; set; } = new List<IdentityRole>();

    public async Task OnGetAsync()
    {
        IdentityRoles = await _context.Roles.ToListAsync();
    }
}
