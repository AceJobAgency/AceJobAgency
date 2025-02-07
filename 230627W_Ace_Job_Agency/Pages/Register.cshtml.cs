using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _230627W_Ace_Job_Agency.Pages {
    public class RegisterModel : PageModel {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        [BindProperty]
        public required User RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync() {
            if (ModelState.IsValid) {
                var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{12,}$");
                if (!passwordRegex.IsMatch(RModel.Password)) {
                    ModelState.AddModelError("RModel.Password", "Password must be at least 12 characters long and include uppercase, lowercase, numbers, and special characters.");
                    return Page();
                }

                string encryptedNRIC = EncryptionHelper.Encrypt(RModel.NRIC);

                string filePath = "";
                if (RModel.Resume != null) {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    filePath = Path.Combine(uploadsFolder, RModel.Resume.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                        await RModel.Resume.CopyToAsync(fileStream);
                    }
                }

                var user = new ApplicationUser {
                    UserName = RModel.Email,
                    Email = RModel.Email,
                    FirstName = RModel.FirstName,
                    LastName = RModel.LastName,
                    Gender = RModel.Gender,
                    NRIC = encryptedNRIC,
                    DateOfBirth = RModel.DateOfBirth,
                    ResumeFileName = filePath,
                    WhoAmI = RModel.WhoAmI
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
