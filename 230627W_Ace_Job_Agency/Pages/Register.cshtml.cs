using System.Net;
using System.Text.RegularExpressions;
using System.Text.Json;
using _230627W_Ace_Job_Agency.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ganss.Xss;

namespace _230627W_Ace_Job_Agency.Pages {
    [ValidateAntiForgeryToken]
    public class RegisterModel : PageModel {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AuthDbContext _context;

        [BindProperty]
        public required User RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AuthDbContext context) {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }

        public IActionResult OnGet() {
            // Initialize RModel with default DateOfBirth
            RModel = new User {
                Email = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                NRIC = string.Empty,
                Resume = new FormFile(Stream.Null, 0, 0, string.Empty, string.Empty),
                FirstName = string.Empty,
                LastName = string.Empty,
                Gender = string.Empty,
                WhoAmI = string.Empty,
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            return Page();
        }

        public async Task<bool> ValidateCaptcha() {
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6LeU9dAqAAAAALkipakj3MLwRoPtw2ropjIq-lvQ";

            using (var httpClient = new HttpClient()) {
                var response = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}", null);
                
                if (response.IsSuccessStatusCode) {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);

                    if (data != null && data.ContainsKey("success") && ((JsonElement)data["success"]).GetBoolean()) {
                        return true;
                    }
                }
                
                return false;
            }
        }

        public async Task<IActionResult> OnPostAsync() {
            if (ModelState.IsValid) {
                bool isCaptchaValid = await ValidateCaptcha();
                if (!isCaptchaValid) {
                    ModelState.AddModelError("", "CAPTCHA check failed.");
                    return Page();
                }

                var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{12,}$");
                if (!passwordRegex.IsMatch(RModel.Password)) {
                    ModelState.AddModelError("RModel.Password", "Password must be at least 12 characters long and include uppercase, lowercase, numbers, and special characters.");
                    return Page();
                }

                string encryptedNRIC = EncryptionHelper.Encrypt(RModel.NRIC);

                string fileName = "";
                if (RModel.Resume != null) {
                    var allowedTypes = new[] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
                    if (!allowedTypes.Contains(RModel.Resume.ContentType)) {
                        ModelState.AddModelError("RModel.Resume", "Only PDF and DOCX files are allowed.");
                        return Page();
                    }

                    string uploadsFolder = Path.Combine("wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    fileName = $"{Guid.NewGuid()}_{RModel.Resume.FileName}";
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                        await RModel.Resume.CopyToAsync(fileStream);
                    }
                }

                var sanitizer = new HtmlSanitizer();

                var sanitisedUsername = sanitizer.Sanitize(RModel.Email);
                if (sanitisedUsername == "") {
                    sanitisedUsername = "N/A";
                }

                var sanitisedEmail = sanitizer.Sanitize(RModel.Email);
                if (sanitisedEmail == "") {
                    sanitisedEmail = "N/A";
                }

                var sanitisedFirstName = sanitizer.Sanitize(RModel.FirstName);
                if (sanitisedFirstName == "") {
                    sanitisedFirstName = "N/A";
                }

                var sanitisedLastName = sanitizer.Sanitize(RModel.LastName);
                if (sanitisedLastName == "") {
                    sanitisedLastName = "N/A";
                }

                var sanitisedNRIC = sanitizer.Sanitize(encryptedNRIC);
                if (sanitisedNRIC == "") {
                    sanitisedNRIC = "N/A";
                }

                var sanitisedResume = sanitizer.Sanitize(fileName);
                if (sanitisedResume == "") {
                    sanitisedResume = "N/A";
                }

                var sanitisedWhoAmI = sanitizer.Sanitize(RModel.WhoAmI);
                if (sanitisedWhoAmI == "") {
                    sanitisedWhoAmI = "N/A";
                }

                var validGenders = new[] { "Male", "Female", "Other" };
                if (!validGenders.Contains(RModel.Gender)) {
                    ModelState.AddModelError("RModel.Gender", "Invalid gender selection.");
                    return Page();
                }

                var user = new ApplicationUser {
                    UserName = sanitisedUsername,
                    Email = sanitisedEmail,
                    FirstName = sanitisedFirstName,
                    LastName = sanitisedLastName,
                    Gender = RModel.Gender,
                    NRIC = sanitisedNRIC,
                    DateOfBirth = RModel.DateOfBirth,
                    ResumeFileName = sanitisedResume,
                    WhoAmI = sanitisedWhoAmI
                };

                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded) {
                    await signInManager.SignInAsync(user, false);
                    
                    if (user != null) {
                        HttpContext.Session.SetString("FirstName", user.FirstName);
                        HttpContext.Session.SetString("LastName", user.LastName);
                        HttpContext.Session.SetString("Gender", user.Gender);
                        HttpContext.Session.SetString("NRIC", EncryptionHelper.Decrypt(user.NRIC));
                        HttpContext.Session.SetString("Email", user.Email ?? string.Empty);
                        HttpContext.Session.SetString("DOB", user.DateOfBirth.ToString("yyyy-MM-dd"));
                        HttpContext.Session.SetString("Resume", user.ResumeFileName ?? "N/A");
                        HttpContext.Session.SetString("WhoAmI", user.WhoAmI);

                        await Logger.LogActivity(RModel.Email, "User registered", _context);
                    }

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
