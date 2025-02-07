using System.ComponentModel.DataAnnotations;

public class User {
    [Required]
    [DataType(DataType.Text)]
    public required string FirstName { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public required string LastName { get; set; }

    [Required]
    public required string Gender { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public required string NRIC { get; set; }

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

    [Required]
    [DataType(DataType.Date)]
    public required DateTime DateOfBirth { get; set; }

    [Required]
    public required string Resume { get; set; }

    [Required]
    public required string WhoAmI { get; set; }
}
