using Microsoft.EntityFrameworkCore;
using Notepad.Database.Data;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;

namespace Notepad.Database.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly NotepadContext _context;

        public NoteRepository(NotepadContext context)
        {
            _context = context;
        }

        public async Task Add(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        public async Task<Note> Get(int id)
        {
            return await _context.Notes
         .Where(n => n.Id == id)
         .Select(n => new Note
         {
             Id = n.Id,
             Title = n.Title,
             Content = n.Content,
             Created = n.Created,
             ImagePath = n.ImagePath,
             Categories = n.Categories.Select(c => new Category
             {
                 Id = c.Id,
                 Name = c.Name
             }).ToList(),
             User = n.User
         })
         .FirstOrDefaultAsync();
        }

        public async Task Edit(Note note)
        {
            var existingEntity = await _context.Notes.FindAsync(note.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Attach(note);
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Note note)
        {
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Note>> GetAll()
        {
            return await _context.Notes.Select(n => new Note
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                Created = n.Created,
                ImagePath = n.ImagePath,
                Categories = n.Categories.Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
                User = n.User
            }).ToListAsync();
        }

        public async Task<List<Note>> GetByTitle(string title)
        {
            var notes = await GetAll();
            return notes.Where(note =>
                        note.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                            .Select(n => new Note
                            {
                                Id = n.Id,
                                Title = n.Title,
                                Content = n.Content,
                                Created = n.Created,
                                ImagePath = n.ImagePath,
                                Categories = n.Categories.Select(c => new Category
                                {
                                    Id = c.Id,
                                    Name = c.Name
                                }).ToList(),
                                User = n.User
                            }).ToList()
                .ToList();
        }

        public async Task<List<Note>> GetByCategory(string category)
        {
            var notes = await GetAll();
            return notes.Where(note =>
                       note.Categories.Any(c => c.Name.Contains(category, StringComparison.OrdinalIgnoreCase))
                                  ).Select(n => new Note
                                  {
                                      Id = n.Id,
                                      Title = n.Title,
                                      Content = n.Content,
                                      Created = n.Created,
                                      ImagePath = n.ImagePath,
                                      Categories = n.Categories.Select(cat => new Category
                                      {
                                          Id = cat.Id,
                                          Name = cat.Name
                                      }).ToList(),
                                      User = n.User
                                  }).ToList()
                .ToList();
        }
    }
}
