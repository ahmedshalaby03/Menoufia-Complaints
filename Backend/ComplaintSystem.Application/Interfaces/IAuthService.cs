using ComplaintSystem.Application.DTOs.Auth;

namespace ComplaintSystem.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
