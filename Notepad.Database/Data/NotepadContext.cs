using Microsoft.EntityFrameworkCore;
using Notepad.Database.Model;

namespace Notepad.Database.Data
{
    public class NotepadContext : DbContext
    {
        public NotepadContext(DbContextOptions<NotepadContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
