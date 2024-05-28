using Notepad.Shared.Dto;

namespace Notepad.Business.Interfaces
{
    public interface IUserService
    {
        Task<string> Login(LoginDto user);
        Task<string> Register(UserDto user);
        Task UpdateUserRole(string userName, RoleType user);
        Task DeleteUserAdminRole(string userName);
        Task ChangePassword(string userName, ChangePasswordDto changePassword);
    }
}