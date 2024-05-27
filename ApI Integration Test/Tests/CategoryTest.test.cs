using ApI_Integration_Test.Data_Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Notepad.Api.Controllers;
using Notepad.Business.Interfaces;
using Notepad.Database.Custom;
using Notepad.Database.Dto;
using Notepad.Database.Model;

namespace ApI_Integration_Test.Tests
{
    public class CategoryTest
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryService> _categoryService;

        public CategoryTest()
        {
            _categoryService = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_categoryService.Object);
        }


        public void Dispose()
        {

        }

        [Theory, CategoryData]
        public async Task AddCategory_SuccessFully(CategoryDto category)
        {
            _categoryService.Setup(x => x.AddCategory(category));

            var result = await _categoryController.Add(category);

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, (result as ObjectResult)!.StatusCode);
        }

        [Theory, CategoryData]
        public async Task GetCategory_SuccessFully(Category categoryOne)
        {

            _categoryService.Setup(x => x.GetCategory(It.IsAny<int>())).ReturnsAsync(categoryOne);

            var result = await _categoryController.Get(It.IsAny<int>());

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, (result as ObjectResult)!.StatusCode);
        }

        [Fact]
        public async Task GetCategory_NotFound()
        {
            _categoryService.Setup(x => x.GetCategory(It.IsAny<int>())).ThrowsAsync(new NotFoundErrorException("Category not found"));

            var result = await _categoryController.Get(It.IsAny<int>());

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, (result as ObjectResult)!.StatusCode);
        }

        [Theory, CategoryData]
        public async Task AddCategory_Conflict(CategoryDto category)
        {

            _categoryService.Setup(x => x.AddCategory(category)).ThrowsAsync(new ConflictErrorException("Category already exists"));

            var result = await _categoryController.Add(category);

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status409Conflict, (result as ObjectResult)!.StatusCode);
        }

        [Theory, CategoryData]
        public async Task UpdateCategory_SuccessFully(CategoryDto category)
        {
            _categoryService.Setup(x => x.UpdateCategory(It.IsAny<int>(), category));

            var result = await _categoryController.Update(It.IsAny<int>(), category);

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, (result as ObjectResult)!.StatusCode);
        }

        [Theory, CategoryData]
        public async Task UpdateCategory_NotFound(CategoryDto category)
        {
            _categoryService.Setup(x => x.UpdateCategory(It.IsAny<int>(), category)).ThrowsAsync(new NotFoundErrorException("Category not found"));

            var result = await _categoryController.Update(It.IsAny<int>(), category);

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, (result as ObjectResult)!.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_SuccessFully()
        {
            _categoryService.Setup(x => x.DeleteCategory(It.IsAny<int>()));

            var result = await _categoryController.Delete(It.IsAny<int>());

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, (result as ObjectResult)!.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_NotFound()
        {
            _categoryService.Setup(x => x.DeleteCategory(It.IsAny<int>())).ThrowsAsync(new NotFoundErrorException("Category not found"));

            var result = await _categoryController.Delete(It.IsAny<int>());

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, (result as ObjectResult)!.StatusCode);
        }
    }
}
