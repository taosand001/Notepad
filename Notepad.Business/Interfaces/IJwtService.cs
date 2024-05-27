using Notepad.Database.Model;

namespace Notepad.Business.Interfaces
{
    public interface IJwtService
    {
        string GenerateSecurityToken(User user);
    }
}