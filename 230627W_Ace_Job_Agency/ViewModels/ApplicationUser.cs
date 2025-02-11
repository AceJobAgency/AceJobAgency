using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser {
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "Invalid characters in name.")]
    public required string FirstName { get; set; }
    
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "Invalid characters in name.")]
    public required string LastName { get; set; }
    public required string Gender { get; set; }

    [RegularExpression(@"^[STFG]\d{7}[A-Z]$", ErrorMessage = "Invalid NRIC format.")]
    public required string NRIC { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string ResumeFileName { get; set; }
    public required string WhoAmI { get; set; }
    public DateTime LastPasswordChange { get; set; }
}
