using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyStarterTemplate.Areas.Identity.Pages;

public class BasePageModel : PageModel
{
    [TempData]
    public string ToastType { get; set; } = string.Empty;
    [TempData]
    public string ToastMessage { get; set; } = string.Empty;
}
