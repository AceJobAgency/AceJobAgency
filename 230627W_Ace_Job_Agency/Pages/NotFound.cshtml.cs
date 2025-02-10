using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _230627W_Ace_Job_Agency.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class NotFoundModel : PageModel {
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<NotFoundModel> _logger;

    public NotFoundModel(ILogger<NotFoundModel> logger) {
        _logger = logger;
    }

    public void OnGet() {
        Response.StatusCode = 404;
    }
}