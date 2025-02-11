using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _230627W_Ace_Job_Agency.Pages;

[Authorize]
public class PrivacyModel : PageModel {
    public PrivacyModel() { }

    public void OnGet() {}
}

