using Microsoft.Extensions.Logging;
using Notepad.Business.Interfaces;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;
using Notepad.Shared.Custom;
using Notepad.Shared.Dto;

namespace Notepad.Business.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task AddCategory(CategoryDto category)
        {
            var existingCategory = await _categoryRepository.Get(0, category.Name);
            if (existingCategory != null)
            {
                _logger.LogError("Category already exists");
                throw new ConflictErrorException("Category already exists");
            }
            var newCategory = new Category
            {
                Name = category.Name,
            };
            await _categoryRepository.Add(newCategory);
        }

        public async Task UpdateCategory(int id, CategoryDto category)
        {
            var existingCategory = await _categoryRepository.Get(id);
            if (existingCategory == null)
            {
                _logger.LogError("Category not found");
                throw new NotFoundErrorException("Category not found");
            }
            existingCategory.Name = category.Name;
            await _categoryRepository.Update(existingCategory);
        }

        public async Task DeleteCategory(int id)
        {
            var existingCategory = await _categoryRepository.Get(id);
            if (existingCategory == null)
            {
                _logger.LogError("Category not found");
                throw new NotFoundErrorException("Category not found");
            }
            await _categoryRepository.Delete(existingCategory);
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _categoryRepository.Get(id);
            if (category == null)
            {
                _logger.LogError("Category not found");
                throw new NotFoundErrorException("Category not found");
            }
            return category;
        }
    }
}
