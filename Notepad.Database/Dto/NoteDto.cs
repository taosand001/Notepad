using Microsoft.AspNetCore.Http;
using Notepad.Database.Attributes;

namespace Notepad.Database.Dto
{
    public class NoteDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        [AllowedExtension([".png", ".jpg"])]
        [MaxFileSize(2 * 1024 * 1024)]
        public IFormFile Image { get; set; }
        public List<string> Categories { get; set; }

        public NoteDto()
        {
        }

        public NoteDto(string title, string content, IFormFile image, List<string> categories)
        {
            Title = title;
            Content = content;
            Categories = categories;
            Image = image;
        }
    }
}
