using Notepad.Database.Model;

namespace Notepad.Database.Interfaces
{
    public interface ICategoryRepository
    {
        Task Add(Category category);
        Task Delete(Category category);
        Task<Category> Get(int id, string name = "");
        Task<List<Category>> GetAll();
        Task Update(Category category);
    }
}