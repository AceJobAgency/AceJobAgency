using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace _230627W_Ace_Job_Agency.Pages {
    public class ChangePasswordModel : PageModel {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ChangePasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public ChangePasswordInputModel Input { get; set; } = new ChangePasswordInputModel();

        public class ChangePasswordInputModel {
            [Required]
            [DataType(DataType.Password)]
            public string CurrentPassword { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [MinLength(12, ErrorMessage = "Password must be at least 12 characters.")]
            public string NewPassword { get; set; } = string.Empty;

            [Required]
            [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password must match.")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToPage("/Login");
            }

            var minPasswordAge = TimeSpan.FromSeconds(30);

            if (DateTime.UtcNow - user.LastPasswordChange < minPasswordAge) {
                ModelState.AddModelError(string.Empty, "You cannot change your password within 30 minutes of your last change.");
                return Page();
            }

            // check if old password and new password are the same
            if (Input.CurrentPassword == Input.NewPassword) {
                ModelState.AddModelError(string.Empty, "New password cannot be the same as the old password.");
                return Page();
            }

            var result = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);
            if (!result.Succeeded) {
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            user.LastPasswordChange = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Password changed successfully!";

            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("LastName", user.LastName);
            HttpContext.Session.SetString("Gender", user.Gender);
            HttpContext.Session.SetString("NRIC", EncryptionHelper.Decrypt(user.NRIC));
            HttpContext.Session.SetString("Email", user.Email ?? string.Empty);
            HttpContext.Session.SetString("DOB", user.DateOfBirth.ToString("yyyy-MM-dd"));
            HttpContext.Session.SetString("Resume", user.ResumeFileName ?? "N/A");
            HttpContext.Session.SetString("WhoAmI", user.WhoAmI);

            return RedirectToPage("Index");
        }
    }
}
