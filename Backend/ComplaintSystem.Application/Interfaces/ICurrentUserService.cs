namespace ComplaintSystem.Application.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Role { get; }
    bool IsAdmin { get; }
}
