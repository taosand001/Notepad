using Microsoft.EntityFrameworkCore;
using Notepad.Database.Data;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;

namespace Notepad.Database.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly NotepadContext _context;

        public UserRepository(NotepadContext context)
        {
            _context = context;
        }

        public async Task<User> Get(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}
