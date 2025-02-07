using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser {
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public required string NRIC { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string ResumeFileName { get; set; }
    public required string WhoAmI { get; set; }
}
