using AutoFixture.Kernel;
using Microsoft.AspNetCore.Http;
using Notepad.Database.Dto;
using Notepad.Database.Model;
using System.Text;

namespace ApI_Integration_Test.Specimen_Builder
{
    public class NoteSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
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

            if (request is Type type && type == typeof(NoteDto))
            {
                return new NoteDto("TestTitle", "TestContent", formFile, new List<string> { "TestCategory" });
            }

            if (request is Type type2 && type2 == typeof(Note))
            {
                var user = new User
                {
                    Id = 1,
                    Username = "TestUser"
                };
                return new Note
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
            }

            return new NoSpecimen();

        }
    }
}
