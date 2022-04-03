using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyStarterTemplate.Data;

namespace MyStarterTemplate.Areas.Identity.ViewModels;

public class RoleNamePageModel : PageModel
{
    public SelectList? RoleNames { get; set; }

    public void PopulateRolesDropDownList(DataContext _context)
    {
        var rolesQuery = from d in _context.Roles
                         orderby d.Name // Sort by name.
                         select d;

        RoleNames = new SelectList(rolesQuery.AsNoTracking(),
                    "Name", "Name");
    }
}
