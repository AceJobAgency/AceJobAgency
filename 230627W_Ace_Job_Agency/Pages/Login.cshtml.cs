using _230627W_Ace_Job_Agency.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace _230627W_Ace_Job_Agency.Pages {
    public class LoginModel : PageModel {
        [BindProperty]
        public required Login LModel { get; set; }
        private readonly SignInManager<ApplicationUser> signInManager;
        public LoginModel(SignInManager<ApplicationUser> signInManager) {
            this.signInManager = signInManager;
        }
        public void OnGet() {}

        public async Task<IActionResult> OnPostAsync() { 
            if (ModelState.IsValid) {
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, false);
                if (identityResult.Succeeded) {
                    HttpContext.Session.SetString("SessionStartTime", DateTime.Now.ToString());
                    return RedirectToPage("Index");
                }
                ModelState.AddModelError("", "Username or Password incorrect");
            }
            return Page();
        }
    }
}