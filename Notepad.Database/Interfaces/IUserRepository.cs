using Notepad.Database.Model;

namespace Notepad.Database.Interfaces
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User> Get(string user);
        Task Update(User user);
    }
}