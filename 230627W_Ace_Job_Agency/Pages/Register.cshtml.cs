using _230627W_Ace_Job_Agency.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _230627W_Ace_Job_Agency.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<IdentityUser> userManager { get; set; }
        private SignInManager<IdentityUser> signInManager { get; set; }

        [BindProperty]
        public required Register RModel { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync() {
            if (ModelState.IsValid) {
                var user = new IdentityUser() {
                    UserName = RModel.Email,
                    Email = RModel.Email
                };
                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded) {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Index");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}