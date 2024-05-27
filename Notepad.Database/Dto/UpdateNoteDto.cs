using Microsoft.AspNetCore.Http;
using Notepad.Database.Attributes;

namespace Notepad.Database.Dto
{
    public class UpdateNoteDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        [AllowedExtension(new string[] { ".png", ".jpg" })]
        [MaxFileSize(2 * 1024 * 1024)]
        public IFormFile Image { get; set; }

        public UpdateNoteDto()
        {
        }

        public UpdateNoteDto(string title, string content, IFormFile image)
        {
            Title = title;
            Content = content;
            Image = image;
        }
    }
}
