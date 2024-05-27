using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Notepad.Database.Attributes
{
    public class AllowedExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            else if (value is string path)
            {
                var extension = Path.GetExtension(path);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success!;
        }

        public string GetErrorMessage()
        {
            return $"Allowed extensions are {string.Join(", ", _extensions)}";
        }
    }
}
