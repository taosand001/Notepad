using Microsoft.EntityFrameworkCore;
using Notepad.Database.Data;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;
using Notepad.Database.Repository;

namespace Database_Unit_Test.Tests
{
    public class CategoryTest
    {
        private readonly NotepadContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryTest()
        {
            var options = new DbContextOptionsBuilder<NotepadContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new NotepadContext(options);
            _categoryRepository = new CategoryRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task AddCategory_SuccessFully()
        {
            var category = new Category
            {
                Name = "TestCategory"
            };

            await _categoryRepository.Add(category);

            var result = await _categoryRepository.Get(1);

            Assert.NotNull(result);
            Assert.Equal("TestCategory", result.Name);
        }

        [Fact]
        public async Task GetCategory_SuccessFully()
        {
            var category = new Category
            {
                Name = "TestCategory"
            };

            await _categoryRepository.Add(category);

            var result = await _categoryRepository.Get(1);

            Assert.NotNull(result);
            Assert.Equal("TestCategory", result.Name);
        }

        [Fact]
        public async Task UpdateCategory_SuccessFully()
        {
            var category = new Category
            {
                Name = "TestCategory"
            };

            await _categoryRepository.Add(category);

            var result = await _categoryRepository.Get(1) ?? null;

            result.Name = "TestCategoryUpdated";

            await _categoryRepository.Update(result);

            var updatedResult = await _categoryRepository.Get(1) ?? null;

            Assert.NotNull(updatedResult);
            Assert.Equal("TestCategoryUpdated", updatedResult.Name);
        }

        [Fact]
        public async Task DeleteCategory_SuccessFully()
        {
            var category = new Category
            {
                Name = "TestCategory"
            };

            await _categoryRepository.Add(category);

            var result = await _categoryRepository.Get(1);

            await _categoryRepository.Delete(result);

            var deletedResult = await _categoryRepository.Get(1);

            Assert.Null(deletedResult);
        }
    }
}
