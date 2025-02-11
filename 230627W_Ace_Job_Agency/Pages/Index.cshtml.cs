using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _230627W_Ace_Job_Agency.Pages;

[Authorize]
public class IndexModel : PageModel {

    public IndexModel() { }

    public void OnGet() {}
}
