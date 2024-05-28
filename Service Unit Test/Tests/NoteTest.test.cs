using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Notepad.Business.Interfaces;
using Notepad.Business.Service;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;
using Notepad.Shared.Custom;
using Notepad.Shared.Dto;
using System.Text;

namespace Service_Unit_Test.Tests
{
    public class NoteTest
    {
        private readonly INoteService _noteService;
        private Mock<INoteRepository> _notemockRepo;
        private Mock<ICategoryRepository> _categorymockRepo;
        private Mock<IUserRepository> _usermockRepo;
        private Mock<ILogger<NoteService>> _logger;

        public NoteTest()
        {
            _notemockRepo = new Mock<INoteRepository>();
            _categorymockRepo = new Mock<ICategoryRepository>();
            _usermockRepo = new Mock<IUserRepository>();
            _logger = new Mock<ILogger<NoteService>>();
            _noteService = new NoteService(_notemockRepo.Object, _usermockRepo.Object, _categorymockRepo.Object, _logger.Object);
        }

        public void Dispose()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos");

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

        }

        [Fact]
        public async Task CreateNote_SuccessFully()
        {
            var content = "Example file content";
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var fileStream = new MemoryStream(contentBytes);
            var formFile = new FormFile(fileStream, 0, contentBytes.Length, "Data", "example.png");

            var note = new NoteDto(title: "TestNote", content: "TestContent", image: formFile, categories: new List<string> { "TestCategory" });

            var userName = "TestUser";

            var user = new User
            {
                Username = userName,
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6]
            };

            _categorymockRepo.Setup(x => x.Get(0, "TestCategory")).ReturnsAsync((Category)null);
            _usermockRepo.Setup(x => x.Get(userName)).ReturnsAsync(user);

            var result = await _noteService.Create(note, userName);

            Assert.NotNull(result);
            Assert.Equal("Note created", result);
        }

        [Fact]
        public async Task CreateNote_CategoryExists_SuccessFully()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var content = "Example file content";
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var fileStream = new MemoryStream(contentBytes);
            var formFile = new FormFile(fileStream, 0, contentBytes.Length, "Data", "example.png");

            var note = new NoteDto(title: "TestNote", content: "TestContent", image: formFile, categories: new List<string> { "TestCategory" });

            var userName = "TestUser";

            var user = new User
            {
                Username = userName,
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6]
            };

            var category = new Category
            {
                Name = "TestCategory"
            };

            _categorymockRepo.Setup(x => x.Get(0, "TestCategory")).ReturnsAsync(category);
            _usermockRepo.Setup(x => x.Get(userName)).ReturnsAsync(user);

            var result = await _noteService.Create(note, userName);

            Assert.NotNull(result);
            Assert.Equal("Note created", result);
        }

        [Fact]
        public async Task CreateNote_UserNotFound()
        {
            var content = "Example file content";
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var fileStream = new MemoryStream(contentBytes);
            var formFile = new FormFile(fileStream, 0, contentBytes.Length, "Data", "example.png");

            var note = new NoteDto(title: "TestNote", content: "TestContent", image: formFile, categories: new List<string> { "TestCategory" });

            var userName = "TestUser";

            _usermockRepo.Setup(x => x.Get(userName)).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _noteService.Create(note, userName));
        }

        [Fact]
        public async Task Update_Note_Successfully()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var content = "Example file content";
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var fileStream = new MemoryStream(contentBytes);
            var formFile = new FormFile(fileStream, 0, contentBytes.Length, "Data", "example.png");

            var note = new UpdateNoteDto(title: "TestNoteUpdated", content: "TestContentUpdated", image: formFile);

            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6]
            };

            _notemockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1))).ReturnsAsync(new Note
            {
                Title = "TestNote",
                Content = "TestContent",
                Created = DateTime.Now,
                ImagePath = "TestImagePath",
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "TestCategory"
                    }
                },
                User = user
            });

            _usermockRepo.Setup(x => x.Get("TestUser")).ReturnsAsync(user);

            var result = await _noteService.Update(1, note, "TestUser");

            Assert.NotNull(result);
            Assert.Equal("Note updated", result);
            _notemockRepo.Verify(x => x.Edit(It.IsAny<Note>()), Times.Once);
        }

        [Fact]
        public async Task Update_Note_NotFound()
        {
            var content = "Example file content";
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var fileStream = new MemoryStream(contentBytes);
            var formFile = new FormFile(fileStream, 0, contentBytes.Length, "Data", "example.png");

            var note = new UpdateNoteDto(title: "TestNoteUpdated", content: "TestContentUpdated", image: formFile);

            _usermockRepo.Setup(x => x.Get("TestUser")).ReturnsAsync(new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6]
            });

            _notemockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1))).ReturnsAsync((Note)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _noteService.Update(1, note, "TestUser"));
        }

        [Fact]
        public async Task Update_Note_Unauthorized()
        {
            var content = "Example file content";
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var fileStream = new MemoryStream(contentBytes);
            var formFile = new FormFile(fileStream, 0, contentBytes.Length, "Data", "example.png");

            var note = new UpdateNoteDto(title: "TestNoteUpdated", content: "TestContentUpdated", image: formFile);

            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6]
            };

            _notemockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1))).ReturnsAsync(new Note
            {
                Title = "TestNote",
                Content = "TestContent",
                Created = DateTime.Now,
                ImagePath = "TestImagePath",
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "TestCategory"
                    }
                },
                User = user
            });

            _usermockRepo.Setup(x => x.Get("TestUserUpdated")).ReturnsAsync(new User
            {
                Username = "TestUserUpdated",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6]
            });

            await Assert.ThrowsAsync<UnauthorizedErrorException>(() => _noteService.Update(1, note, "TestUserUpdated"));
        }

        [Fact]
        public async Task Update_Note_UserNotFound()
        {
            var content = "Example file content";
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var fileStream = new MemoryStream(contentBytes);
            var formFile = new FormFile(fileStream, 0, contentBytes.Length, "Data", "example.png");

            var note = new UpdateNoteDto(title: "TestNoteUpdated", content: "TestContentUpdated", image: formFile);

            _usermockRepo.Setup(x => x.Get("TestUser")).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _noteService.Update(1, note, "TestUser"));
        }

        [Fact]
        public async Task Delete_Note_Successfully()
        {
            var user = new User
            {
                Username = "TestUser",
                PasswordHash = [1, 2, 3],
                PasswordSalt = [4, 5, 6]
            };

            _usermockRepo.Setup(x => x.Get("TestUser")).ReturnsAsync(user);


            var note = new Note
            {
                Title = "TestNote",
                Content = "TestContent",
                Created = DateTime.Now,
                ImagePath = "TestImagePath",
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "TestCategory"
                    }
                },
                User = user
            };

            _notemockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1))).ReturnsAsync(note);

            var result = await _noteService.Delete(1, user.Username);

            Assert.NotNull(result);
            Assert.Equal("Note deleted", result);
        }

        [Fact]
        public async Task Delete_Note_NotFound()
        {
            _notemockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1))).ReturnsAsync((Note)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _noteService.Delete(1, "TestUser"));
        }

        [Fact]
        public async Task GetAll_Notes_Successfully()
        {
            var notes = new List<Note>
            {
                new Note
                {
                    Title = "TestNote",
                    Content = "TestContent",
                    Created = DateTime.Now,
                    ImagePath = "TestImagePath",
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            Name = "TestCategory"
                        }
                    },
                    User = new User
                    {
                        Username = "TestUser",
                        PasswordHash = [1, 2, 3],
                        PasswordSalt = [4, 5, 6]
                    }
                }
            };

            _notemockRepo.Setup(x => x.GetAll()).ReturnsAsync(notes);

            var result = await _noteService.GetAll();

            Assert.NotNull(result);
            Assert.Equal(notes, result);
            Assert.True(result.Count() == 1);
        }

        [Fact]
        public async Task GetByTitle_Notes_Successfully()
        {
            var notes = new List<Note>
            {
                new Note
                {
                    Title = "TestNote",
                    Content = "TestContent",
                    Created = DateTime.Now,
                    ImagePath = "TestImagePath",
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            Name = "TestCategory"
                        }
                    },
                    User = new User
                    {
                        Username = "TestUser",
                        PasswordHash = [1, 2, 3],
                        PasswordSalt = [4, 5, 6]
                    }
                }
            };

            _notemockRepo.Setup(x => x.GetByTitle("TestNote")).ReturnsAsync(notes);

            var result = await _noteService.GetByTitle("TestNote");

            Assert.NotNull(result);
            Assert.Equal(notes, result);
            Assert.True(result.Count == 1);
        }

        [Fact]
        public async Task GetByCategory_Notes_Successfully()
        {
            var notes = new List<Note>
            {
                new Note
                {
                    Title = "TestNote",
                    Content = "TestContent",
                    Created = DateTime.Now,
                    ImagePath = "TestImagePath",
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            Name = "TestCategory"
                        }
                    },
                    User = new User
                    {
                        Username = "TestUser",
                        PasswordHash = [1, 2, 3],
                        PasswordSalt = [4, 5, 6]
                    }
                }
            };

            _notemockRepo.Setup(x => x.GetByCategory("TestCategory")).ReturnsAsync(notes);

            var result = await _noteService.GetByCategory("TestCategory");

            Assert.NotNull(result);
            Assert.Equal(notes, result);
            Assert.True(result.Count == 1);
        }

        [Fact]
        public async Task GetById_Note_Successfully()
        {
            var note = new Note
            {
                Title = "TestNote",
                Content = "TestContent",
                Created = DateTime.Now,
                ImagePath = "TestImagePath",
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "TestCategory"
                    }
                },
                User = new User
                {
                    Username = "TestUser",
                    PasswordHash = [1, 2, 3],
                    PasswordSalt = [4, 5, 6]
                }
            };

            _notemockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1))).ReturnsAsync(note);

            var result = await _noteService.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(note, result);
        }

        [Fact]
        public async Task GetById_Note_NotFound()
        {
            _notemockRepo.Setup(x => x.Get(It.Is<int>(id => id == 1))).ReturnsAsync((Note)null);

            await Assert.ThrowsAsync<NotFoundErrorException>(() => _noteService.GetById(1));
        }
    }
}
