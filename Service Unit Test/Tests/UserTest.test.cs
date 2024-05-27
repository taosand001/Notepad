using Microsoft.Extensions.Logging;
using Moq;
using Notepad.Business.Interfaces;
using Notepad.Business.Service;
using Notepad.Database.Custom;
using Notepad.Database.Dto;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;

namespace Service_Unit_Test.Tests
{
    public class UserTest
    {
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _userMock;
        private readonly Mock<IJwtService> _jwtMock;
        private readonly Mock<IPasswordHashingService> _wordHashingServiceMock;
        private readonly Mock<ILogger<UserService>> _logger;

        public UserTest()
        {
            _userMock = new Mock<IUserRepository>();
            _jwtMock = new Mock<IJwtService>();
            _wordHashingServiceMock = new Mock<IPasswordHashingService>();
            _logger = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userMock.Object, _jwtMock.Object, _wordHashingServiceMock.Object, _logger.Object);
        }

        [Fact]
        public async Task AddUser_SuccessFully()
        {
            var user = new UserDto(Username: "TestUser", Password: "TestPassword");

            await _userService.Register(user);

            _userMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
        }


        [Fact]
        public async Task Login_User_Successfully()
        {


            var user = new LoginDto(Username: "TestUser", Password: "TestPassword");

            _userMock.Setup(_userMock => _userMock.Get(user.Username)).ReturnsAsync(new User
            {
                Username = user.Username,
                PasswordHash = [1, 2, 3],
                PasswordSalt = [1, 2, 3]
            });

            _wordHashingServiceMock.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);

            await _userService.Login(user);

            _jwtMock.Verify(x => x.GenerateSecurityToken(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Login_User_Fail()
        {
            var user = new LoginDto(Username: "TestUser", Password: "TestPassword");

            _userMock.Setup(_userMock => _userMock.Get(user.Username)).ReturnsAsync(new User
            {
                Username = user.Username,
                PasswordHash = [1, 2, 3],
                PasswordSalt = [1, 2, 3]
            });

            _wordHashingServiceMock.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

            await Assert.ThrowsAsync<UnauthorizedErrorException>(() => _userService.Login(user));

        }

        [Fact]
        public async Task Register_User_Fail()
        {
            var user = new UserDto(Username: "TestUser", Password: "TestPassword");

            _userMock.Setup(_userMock => _userMock.Get(user.Username)).ReturnsAsync(new User
            {
                Username = user.Username,
                PasswordHash = [1, 2, 3],
                PasswordSalt = [1, 2, 3]
            });

            await Assert.ThrowsAsync<ConflictErrorException>(() => _userService.Register(user));
        }

        [Fact]
        public async Task UpdateUserRole_SuccessFully()
        {
            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [1, 2, 3]
            };

            _userMock.Setup(_userMock => _userMock.Get(user.Username)).ReturnsAsync(user);

            await _userService.UpdateUserRole(user.Username, RoleType.Admin);

            _userMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserRole_Fail()
        {
            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [1, 2, 3]
            };

            _userMock.Setup(_userMock => _userMock.Get(user.Username))!.ReturnsAsync((User)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _userService.UpdateUserRole(user.Username, RoleType.Admin));
        }

        [Fact]
        public async Task DeleteAdminRole_Successfully()
        {
            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [1, 2, 3]
            };

            _userMock.Setup(_userMock => _userMock.Get(user.Username)).ReturnsAsync(user);

            await _userService.DeleteUserAdminRole(user.Username);

            _userMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAdminRole_Fail()
        {
            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [1, 2, 3]
            };

            _userMock.Setup(_userMock => _userMock.Get(user.Username))!.ReturnsAsync((User)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _userService.DeleteUserAdminRole(user.Username));
        }

    }
}
