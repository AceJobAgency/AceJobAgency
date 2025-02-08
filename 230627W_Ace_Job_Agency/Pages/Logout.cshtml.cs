using _230627W_Ace_Job_Agency.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace _230627W_Ace_Job_Agency.Pages {
    public class LogoutModel : PageModel {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AuthDbContext _context;
        public LogoutModel(SignInManager<ApplicationUser> signInManager, AuthDbContext context) {
            this.signInManager = signInManager;
            _context = context;
        }
        public void OnGet() {}

        public async Task<IActionResult> OnPostLogoutAsync() {
            var email = HttpContext.Session.GetString("Email");
            await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            if (email != null) {
                await Logger.LogActivity(email, "User logged out", _context);
            }
            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostDontLogoutAsync() {
            return await Task.FromResult(RedirectToPage("Index"));
        }
    }
}