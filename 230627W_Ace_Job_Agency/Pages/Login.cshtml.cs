using _230627W_Ace_Job_Agency.Model;
using _230627W_Ace_Job_Agency.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _230627W_Ace_Job_Agency.Pages {
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel {
        [BindProperty]
        public required Login LModel { get; set; }
        
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AuthDbContext context) {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public void OnGet() {}
        public async Task<IActionResult> OnPostAsync() { 
            if (ModelState.IsValid)  {
                var user = await _userManager.FindByEmailAsync(LModel.Email);

                if (user == null) {
                    ModelState.AddModelError("", "Invalid credentials");
                    return Page();
                } else {
                    var identityResult = await _signInManager.PasswordSignInAsync(
                        LModel.Email, 
                        LModel.Password, 
                        LModel.RememberMe, 
                        lockoutOnFailure: true
                    );

                    if (identityResult.Succeeded)  {
                        if (user != null) {
                            HttpContext.Session.Clear();
                            await HttpContext.Session.CommitAsync();
                            
                            HttpContext.Session.SetString("FirstName", user.FirstName);
                            HttpContext.Session.SetString("LastName", user.LastName);
                            HttpContext.Session.SetString("Gender", user.Gender);
                            HttpContext.Session.SetString("NRIC", EncryptionHelper.Decrypt(user.NRIC));
                            HttpContext.Session.SetString("Email", user.Email ?? string.Empty);
                            HttpContext.Session.SetString("DOB", user.DateOfBirth.ToString("yyyy-MM-dd"));
                            HttpContext.Session.SetString("Resume", user.ResumeFileName ?? "N/A");
                            HttpContext.Session.SetString("WhoAmI", user.WhoAmI);

                            await Logger.LogActivity(LModel.Email, "User logged in", _context);
                        } else {
                            ModelState.AddModelError("", "User not found");
                            return Page();
                        }

                        return RedirectToPage("Index");
                    } else if (identityResult.IsLockedOut)  {
                        ModelState.AddModelError("", "Account is locked out due to multiple failed attempts. Please try again later.");
                        await Logger.LogActivity(LModel.Email, "Account locked out.", _context);
                    } else {
                        ModelState.AddModelError("", "Invalid credentials");
                        await Logger.LogActivity(LModel.Email, "Access denied", _context);
                    }
                }
            }
            return Page();
        }
    }
}