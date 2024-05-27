using Microsoft.EntityFrameworkCore;
using Notepad.Database.Data;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;
using Notepad.Database.Repository;

namespace Database_Unit_Test.Tests
{
    public class NoteTest
    {
        private readonly NotepadContext _context;
        private readonly INoteRepository _noteRepository;
        private readonly DbContextOptions<NotepadContext> _options;

        public NoteTest()
        {
            _options = new DbContextOptionsBuilder<NotepadContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new NotepadContext(_options);
            _noteRepository = new NoteRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddNote_SuccessFully()
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

            await _noteRepository.Add(note);

            var result = await _noteRepository.Get(1);

            Assert.NotNull(result);
            Assert.Equal("TestNote", result.Title);
        }

        [Fact]
        public async Task GetNote_SuccessFully()
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

            await _noteRepository.Add(note);

            var result = await _noteRepository.Get(1);

            Assert.NotNull(result);
            Assert.Equal("TestNote", result.Title);
        }

        [Fact]
        public async Task UpdateNote_SuccessFully()
        {
            //var note = new Note
            //{
            //    Title = "TestNote",
            //    Content = "TestContent",
            //    Created = DateTime.Now,
            //    ImagePath = "TestImagePath",
            //    Categories = new List<Category>
            //    {
            //        new Category
            //        {
            //            Name = "TestCategory"
            //        }
            //    },
            //    User = new User
            //    {
            //        Username = "TestUser",
            //        PasswordHash = [1, 2, 3],
            //        PasswordSalt = [4, 5, 6]
            //    }
            //};

            //await _noteRepository.Add(note);

            //await _context.DisposeAsync();
            //using var updateContext = new NotepadContext(_options);
            //var noteRepositoryForUpdate = new NoteRepository(updateContext);

            //var result = await noteRepositoryForUpdate.Get(1);

            //result.Title = "TestNoteUpdated";
            //result.Content = "TestContentUpdated";
            //result.ImagePath = "TestImagePathUpdated";

            //await _noteRepository.Edit(result);

            //var updatedResult = await _noteRepository.Get(1);

            //Assert.NotNull(updatedResult);
            //Assert.Equal("TestNoteUpdated", updatedResult.Title);
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
                    PasswordHash = new byte[] { 1, 2, 3 },
                    PasswordSalt = new byte[] { 4, 5, 6 }
                }
            };

            await _noteRepository.Add(note);

            // Create a new context for updating the note
            using (var updateContext = new NotepadContext(_options))
            {
                var noteRepositoryForUpdate = new NoteRepository(updateContext);
                var result = await noteRepositoryForUpdate.Get(1);

                result.Title = "TestNoteUpdated";
                result.Content = "TestContentUpdated";
                result.ImagePath = "TestImagePathUpdated";

                await noteRepositoryForUpdate.Edit(result);
            }

            // Use a fresh context to verify the update
            using (var verifyContext = new NotepadContext(_options))
            {
                var noteRepositoryForVerification = new NoteRepository(verifyContext);
                var updatedResult = await noteRepositoryForVerification.Get(1);

                Assert.NotNull(updatedResult);
                Assert.Equal("TestNoteUpdated", updatedResult.Title);
            }
        }
    }
}
