using System.ComponentModel.DataAnnotations;

public class User {
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
    public required string ConfirmPassword { get; set; }

    public required string NRIC { get; set; }
    public required IFormFile Resume { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string WhoAmI { get; set; }
}
