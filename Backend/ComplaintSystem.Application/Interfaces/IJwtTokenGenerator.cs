using ComplaintSystem.Domain.Entities;

namespace ComplaintSystem.Application.Interfaces;

public interface IJwtTokenGenerator
{
    (string token, DateTime expiresAt) GenerateToken(ApplicationUser user, IList<string> roles);
}
