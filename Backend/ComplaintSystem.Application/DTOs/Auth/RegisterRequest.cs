namespace ComplaintSystem.Application.DTOs.Auth;

public class RegisterRequest
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = default!; // Admin / Employee
}
