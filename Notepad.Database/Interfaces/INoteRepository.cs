using Notepad.Database.Model;

namespace Notepad.Database.Interfaces
{
    public interface INoteRepository
    {
        Task Add(Note note);
        Task<Note> Get(int id);
        Task Edit(Note note);
        Task Delete(Note note);
        Task<List<Note>> GetAll();
        Task<List<Note>> GetByTitle(string title);
        Task<List<Note>> GetByCategory(string category);
    }
}