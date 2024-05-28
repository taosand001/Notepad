using Notepad.Database.Model;
using Notepad.Shared.Dto;

namespace Notepad.Business.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategory(CategoryDto category);
        Task DeleteCategory(int id);
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategory(int id);
        Task UpdateCategory(int id, CategoryDto category);
    }
}