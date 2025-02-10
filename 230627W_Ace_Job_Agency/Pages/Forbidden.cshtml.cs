using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _230627W_Ace_Job_Agency.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ForbiddenModel : PageModel {
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ForbiddenModel> _logger;

    public ForbiddenModel(ILogger<ForbiddenModel> logger) {
        _logger = logger;
    }

    public void OnGet() {
        Response.StatusCode = 403;
    }
}