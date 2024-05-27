using Microsoft.AspNetCore.Mvc;
using Moq;
using Notepad.Api.Controllers;
using Notepad.Business.Interfaces;
using Notepad.Database.Custom;
using Notepad.Database.Dto;

namespace ApI_Integration_Test.Tests
{
    public class UserTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _userController;

        public UserTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task AddUser_SuccessFully()
        {
            var user = new UserDto(Username: "TestUser", Password: "TestPassword");

            _userServiceMock.Setup(x => x.Register(user)).ReturnsAsync("User created successfully");

            var result = await _userController.Register(user);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task GetUser_SuccessFully()
        {
            var user = new LoginDto(Username: "TestUser", Password: "TestPassword");

            _userServiceMock.Setup(x => x.Login(user)).ReturnsAsync("123456789000000");

            var result = await _userController.Login(user);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("123456789000000", (result as OkObjectResult)!.Value);
        }

        [Fact]
        public async Task UpdateRole_SuccessFully()
        {
            var username = "TestUser";

            _userServiceMock.Setup(x => x.UpdateUserRole(username, RoleType.Admin));

            var result = await _userController.UpdateUserRole(username, RoleType.Admin);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateRole_NotFound()
        {
            var username = "TestUser";

            _userServiceMock.Setup(x => x.UpdateUserRole(username, RoleType.Admin)).ThrowsAsync(new NotFoundErrorException("User is not found"));

            var result = await _userController.UpdateUserRole(username, RoleType.Admin);

            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal("User is not found", (result as ObjectResult)!.Value);
        }

        [Fact]
        public async Task AddUser_Conflict()
        {
            var user = new UserDto(Username: "TestUser", Password: "TestPassword");

            _userServiceMock.Setup(x => x.Register(user)).ThrowsAsync(new ConflictErrorException("User already exists"));

            var result = await _userController.Register(user);

            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            Assert.Equal("User already exists", (result as ObjectResult)!.Value);
        }
    }
}
