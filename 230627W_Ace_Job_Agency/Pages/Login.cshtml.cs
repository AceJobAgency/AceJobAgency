using _230627W_Ace_Job_Agency.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _230627W_Ace_Job_Agency.Pages {
    public class LoginModel : PageModel {
        [BindProperty]
        public required Login LModel { get; set; }
        
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public void OnGet() {}

        public async Task<IActionResult> OnPostAsync() { 
            if (ModelState.IsValid)  {
                var identityResult = await _signInManager.PasswordSignInAsync(
                    LModel.Email, 
                    LModel.Password, 
                    LModel.RememberMe, 
                    false
                );

                if (identityResult.Succeeded)  {
                    var user = await _userManager.FindByEmailAsync(LModel.Email);
                    
                    if (user != null) {
                        HttpContext.Session.SetString("FirstName", user.FirstName);
                        HttpContext.Session.SetString("LastName", user.LastName);
                        HttpContext.Session.SetString("Gender", user.Gender);
                        HttpContext.Session.SetString("NRIC", EncryptionHelper.Decrypt(user.NRIC));
                        HttpContext.Session.SetString("Email", user.Email ?? string.Empty);
                        HttpContext.Session.SetString("DOB", user.DateOfBirth.ToString("yyyy-MM-dd"));
                        HttpContext.Session.SetString("Resume", user.ResumeFileName ?? "N/A");
                        HttpContext.Session.SetString("WhoAmI", user.WhoAmI);
                    } else {
                        ModelState.AddModelError("", "User not found");
                        return Page();
                    }

                    return RedirectToPage("Index");
                }
                ModelState.AddModelError("", "Invalid credentials");
            }
            return Page();
        }
    }
}