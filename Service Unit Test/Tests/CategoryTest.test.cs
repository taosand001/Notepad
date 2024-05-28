using Microsoft.Extensions.Logging;
using Moq;
using Notepad.Business.Interfaces;
using Notepad.Business.Service;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;
using Notepad.Shared.Custom;
using Notepad.Shared.Dto;

namespace Service_Unit_Test.Tests
{
    public class CategoryTest
    {
        private Mock<ICategoryRepository> _categorymockRepo;
        private ICategoryService _categoryService;
        private readonly Mock<ILogger<CategoryService>> _logger;

        public CategoryTest()
        {
            _categorymockRepo = new Mock<ICategoryRepository>();
            _logger = new Mock<ILogger<CategoryService>>();
            _categoryService = new CategoryService(_categorymockRepo.Object, _logger.Object);
        }

        [Fact]
        public async Task AddCategory_SuccessFully()
        {
            var category = new CategoryDto(Name: "TestCategory");

            _categorymockRepo.Setup(x => x.Get(0, "TestCategory")).ReturnsAsync((Category)null);

            await _categoryService.AddCategory(category);

            _categorymockRepo.Verify(x => x.Add(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_SuccessFully()
        {
            var category = new CategoryDto(Name: "TestCategory");

            _categorymockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1), "")).ReturnsAsync(new Category { Id = 1, Name = "TestCategory" });

            await _categoryService.UpdateCategory(1, category);

            _categorymockRepo.Verify(x => x.Update(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_SuccessFully()
        {
            _categorymockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1), "")).ReturnsAsync(new Category { Id = 1, Name = "TestCategory" });

            await _categoryService.DeleteCategory(1);

            _categorymockRepo.Verify(x => x.Delete(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task GetAllCategories_SuccessFully()
        {
            _categorymockRepo.Setup(x => x.GetAll()).ReturnsAsync(new List<Category>());

            var result = await _categoryService.GetAllCategories();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCategory_SuccessFully()
        {
            _categorymockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1), "")).ReturnsAsync(new Category { Id = 1, Name = "TestCategory" });

            var result = await _categoryService.GetCategory(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCategory_NotFound()
        {
            _categorymockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1), "")).ReturnsAsync((Category)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _categoryService.GetCategory(1));
        }

        [Fact]
        public async Task UpdateCategory_NotFound()
        {
            var category = new CategoryDto(Name: "TestCategory");

            _categorymockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1), "")).ReturnsAsync((Category)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _categoryService.UpdateCategory(1, category));
        }

        [Fact]
        public async Task DeleteCategory_NotFound()
        {
            _categorymockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1), "")).ReturnsAsync((Category)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _categoryService.DeleteCategory(1));
        }
    }
}
