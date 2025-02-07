using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace _230627W_Ace_Job_Agency.Pages {
    public class LogoutModel : PageModel {
        private readonly SignInManager<ApplicationUser> signInManager;
        public LogoutModel(SignInManager<ApplicationUser> signInManager) {
            this.signInManager = signInManager;
        }
        public void OnGet() {}

        public async Task<IActionResult> OnPostLogoutAsync() {
            await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostDontLogoutAsync() {
            return await Task.FromResult(RedirectToPage("Index"));
        }
    }
}