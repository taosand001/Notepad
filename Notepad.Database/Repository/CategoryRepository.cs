using Microsoft.EntityFrameworkCore;
using Notepad.Database.Data;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;

namespace Notepad.Database.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NotepadContext _context;

        public CategoryRepository(NotepadContext context)
        {
            _context = context;
        }

        public async Task Add(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> Get(int id = 0, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Notes = c.Notes.Select(n => new Note
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    ImagePath = n.ImagePath,
                    User = new User
                    {
                        Id = n.User.Id,
                        Username = n.User.Username
                    }
                }).ToList()
            })
                .ToListAsync();
        }

        public async Task Update(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
