using Microsoft.EntityFrameworkCore;
using Notepad.Database.Data;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;
using Notepad.Database.Repository;
using Notepad.Shared.Dto;

namespace Database_Unit_Test.Tests
{
    public class UserTest
    {
        private readonly NotepadContext _context;
        private readonly IUserRepository _userRepository;

        public UserTest()
        {
            var options = new DbContextOptionsBuilder<NotepadContext>()
               .UseInMemoryDatabase(databaseName: "NotepadTest")
               .Options;
            _context = new NotepadContext(options);
            _userRepository = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddUser_SuccessFully()
        {
            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6],
            };

            await _userRepository.Add(user);

            var result = await _userRepository.Get("TestUser");

            Assert.NotNull(result);
            Assert.Equal("TestUser", result.Username);
        }

        [Fact]
        public async Task GetUser_SuccessFully()
        {
            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6],
            };

            await _userRepository.Add(user);

            var result = await _userRepository.Get("TestUser");

            Assert.NotNull(result);
            Assert.Equal("TestUser", result.Username);
        }

        [Fact]
        public async Task UpdateUser_SuccessFully()
        {
            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6],
            };

            await _userRepository.Add(user);

            var result = await _userRepository.Get("TestUser");

            result.Username = "TestUserUpdated";

            await _userRepository.Update(result);

            var updatedResult = await _userRepository.Get("TestUserUpdated");

            Assert.NotNull(updatedResult);
            Assert.Equal("TestUserUpdated", updatedResult.Username);
        }

        [Fact]
        public async Task UpdateUserRole()
        {
            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6],
            };

            await _userRepository.Add(user);

            var result = await _userRepository.Get("TestUser");

            result.Role = RoleType.Admin;

            await _userRepository.Update(result);

            var updatedResult = await _userRepository.Get("TestUser");

            Assert.NotNull(updatedResult);
            Assert.Equal("Admin", updatedResult.Role.ToString());
        }
    }
}