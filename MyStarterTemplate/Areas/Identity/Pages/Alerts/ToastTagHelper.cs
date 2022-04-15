using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace MyStarterTemplate.Areas.Identity.Pages.Alerts;

[HtmlTargetElement("div", Attributes = "toasts")]
public class ToastTagHelper : TagHelper
{
    [ViewContext]
    public ViewContext ViewContext { get; set; } = new();

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        StringBuilder str = new ();

        string? toastType = ViewContext.TempData["ToastType"] as string;
        string? toastMessage = ViewContext.TempData["ToastMessage"] as string;
        if (!string.IsNullOrWhiteSpace(toastType) && !string.IsNullOrWhiteSpace(toastMessage))
        {
            str.Append($"<div id='toastMessage' class='toast align-items-center text-white bg-{toastType} border-0'" +
                $"role='alert' aria-live='assertive' aria-atomic='true'>");
            str.Append("<div class='d-flex'>");
            str.Append("<div class='toast-body m-1'>");

                str.Append(toastMessage);

            str.Append("</div>");
            str.Append("<button type='button' class='btn-close btn-close-white " +
                "me-2 m-auto' data-bs-dismiss='toast' aria-label='Close'></button>");
            str.Append("</div>");
            str.Append("</div>");
        }
        output.Content.AppendHtml(str.ToString());
    }
}
