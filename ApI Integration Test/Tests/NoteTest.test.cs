using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Notepad.Api.Controllers;
using Notepad.Business.Interfaces;
using Notepad.Database.Custom;
using Notepad.Database.Dto;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;
using System.Security.Claims;
using System.Text;

namespace ApI_Integration_Test.Tests
{
    public class NoteTest
    {
        private readonly Mock<INoteService> _noteServiceMock;
        private readonly Mock<IUserRepository> _userMockRepo;
        private readonly NoteController _noteController;

        public NoteTest()
        {
            _noteServiceMock = new Mock<INoteService>();
            _userMockRepo = new Mock<IUserRepository>();
            _noteController = new NoteController(_noteServiceMock.Object);
        }

        public void Dispose()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos");

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath);
            }
        }

        [Fact]
        public async Task AddNote_SuccessFully()
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

            var note = new NoteDto("TestTitle", "TestContent", formFile, new List<string> { "TestCategory" });

            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Name, "TestUser")
               }));

            _noteController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };

            _noteServiceMock.Setup(x => x.Create(It.IsAny<NoteDto>(), "TestUser")).ReturnsAsync("Note created");

            var result = await _noteController.Add(note);

            var okResult = result as CreatedResult;

            Assert.NotNull(okResult);
            Assert.Equal(201, okResult.StatusCode);
        }

        [Fact]
        public async Task AddNote_Fail()
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

            var note = new NoteDto("TestTitle", "TestContent", formFile, new List<string> { "TestCategory" });

            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, "TestUser")
               }));

            _noteController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };

            _noteServiceMock.Setup(x => x.Create(It.IsAny<NoteDto>(), "TestUser")).ThrowsAsync(new NotFoundErrorException("User not found"));

            var result = await _noteController.Add(note);

            var badRequestResult = result as ObjectResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetNote_SuccessFully()
        {
            var user = new User
            {
                Id = 1,
                Username = "TestUser"
            };
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
            _noteServiceMock.Setup(x => x.GetById(It.Is<int>(id => id == 1))).ReturnsAsync(note);

            var result = await _noteController.Get(1);

            var okResult = result as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetNote_NotFound()
        {
            _noteServiceMock.Setup(x => x.GetById(It.Is<int>(id => id == 1))).ThrowsAsync(new NotFoundErrorException("Note not found"));

            var result = await _noteController.Get(1);

            var notFoundResult = result as ObjectResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateNote_SuccessFully()
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

            var note = new UpdateNoteDto("TestTitle", "TestContent", formFile);

            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, "TestUser")
               }));

            _noteController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };

            _noteServiceMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateNoteDto>(), "TestUser")).ReturnsAsync("Note updated");

            var result = await _noteController.Update(1, note);

            var okResult = result as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateNote_Fail()
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

            var note = new UpdateNoteDto("TestTitle", "TestContent", formFile);

            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, "TestUser")
               }));

            _noteController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };

            _noteServiceMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<UpdateNoteDto>(), "TestUser")).ThrowsAsync(new NotFoundErrorException("Note not found"));

            var result = await _noteController.Update(1, note);

            var notFoundResult = result as ObjectResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteNote_SuccessFully()
        {

            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, "TestUser")
               }));

            _noteController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };

            _noteServiceMock.Setup(x => x.Delete(It.Is<int>(id => id == 1), "TestUser")).ReturnsAsync("Note deleted");

            var result = await _noteController.Delete(1);

            var okResult = result as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteNote_NotFound()
        {

            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, "TestUser")
               }));

            _noteController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };
            _noteServiceMock.Setup(x => x.Delete(It.Is<int>(id => id == 1), "TestUser")).ThrowsAsync(new NotFoundErrorException("Note not found"));

            var result = await _noteController.Delete(1);

            var notFoundResult = result as ObjectResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
