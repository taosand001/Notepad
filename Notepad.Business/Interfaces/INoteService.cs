using Notepad.Database.Model;
using Notepad.Shared.Dto;

namespace Notepad.Business.Interfaces
{
    public interface INoteService
    {
        Task<string> Create(NoteDto note, string userName);
        Task<string> Delete(int id, string username);
        Task<IEnumerable<Note>> GetAll();
        Task<List<Note>> GetByCategory(string category);
        Task<Note> GetById(int id);
        Task<List<Note>> GetByTitle(string title);
        Task<string> Update(int id, UpdateNoteDto note, string username);
        Task<List<Note>> GetByUser(string username);
    }
}