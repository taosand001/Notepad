using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Notepad.Business.Interfaces;
using Notepad.Database.Interfaces;
using Notepad.Database.Model;
using Notepad.Shared.Custom;
using Notepad.Shared.Dto;

namespace Notepad.Business.Service
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<NoteService> _logger;

        public NoteService(INoteRepository noteRepository, IUserRepository userRepository, ICategoryRepository categoryRepository, ILogger<NoteService> logger)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<string> Create(NoteDto note, string userName)
        {
            try
            {
                var user = await _userRepository.Get(userName);
                if (user == null)
                {
                    throw new NotFoundErrorException("User not found");
                }

                List<Category> categories = new List<Category>();

                foreach (string category in note.Categories)
                {
                    var existingCategory = await _categoryRepository.Get(0, category);
                    if (existingCategory == null)
                    {
                        var newCategory = new Category
                        {
                            Name = category,
                        };
                        await _categoryRepository.Add(newCategory);
                        categories.Add(newCategory);
                    }
                    else
                    {
                        categories.Add(existingCategory);
                    }
                }

                var filePath = await PhotoLogic(note.Image);

                var noteEntity = new Note
                {
                    Title = note.Title,
                    Content = note.Content,
                    User = user,
                    Categories = categories,
                    ImagePath = filePath
                };

                await _noteRepository.Add(noteEntity);
                _logger.LogInformation("Note created");
                _logger.LogInformation("Note created by {userName}", userName);
                return "Note created";
            }
            catch (NotFoundErrorException Ex)
            {
                _logger.LogError(Ex, Ex.Message.ToString());
                throw new NotFoundErrorException(Ex.Message);
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex, Ex.Message.ToString());
                throw new Exception(Ex.Message);
            }
        }

        private async Task<string> PhotoLogic(IFormFile photo)
        {
            string folderName = "photos";
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = $"{folderName}/{photo.Name}_{DateTime.Now.ToString("yyyy_MM_dd_H_m_s")}{Path.GetExtension(photo.FileName)}";
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }
            _logger.LogInformation($"Picture has been created");
            return filePath;
        }

        public async Task<string> Update(int id, UpdateNoteDto note, string username)
        {
            var user = await _userRepository.Get(username);
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new NotFoundErrorException("User not found");
            }

            var noteEntity = await _noteRepository.Get(id);
            if (noteEntity == null)
            {
                _logger.LogError("Note not found");
                throw new NotFoundErrorException("Note not found");
            }

            if (user.Username != noteEntity.User!.Username)
            {
                _logger.LogError("User not authorized to update note");
                throw new UnauthorizedErrorException("User not authorized to update note");
            }

            noteEntity.Content = note.Content;
            noteEntity.Title = note.Title;
            noteEntity.ImagePath = await PhotoLogic(note.Image);
            await _noteRepository.Edit(noteEntity);
            _logger.LogInformation("Note was updated by {username}", username);
            return "Note updated";
        }

        public async Task<string> Delete(int id, string username)
        {
            var user = await _userRepository.Get(username);
            var noteEntity = await _noteRepository.Get(id);
            if (noteEntity == null)
            {
                throw new NotFoundErrorException("Note not found");
            }

            if (noteEntity.User.Username != user.Username)
            {
                _logger.LogError("User not authorized to update note");
                throw new UnauthorizedErrorException("User not authorized to update note");
            }

            await _noteRepository.Delete(noteEntity);
            return "Note deleted";
        }

        public async Task<IEnumerable<Note>> GetAll()
        {
            return await _noteRepository.GetAll();

        }

        public async Task<List<Note>> GetByTitle(string title)
        {
            return await _noteRepository.GetByTitle(title);
        }

        public async Task<List<Note>> GetByCategory(string category)
        {
            return await _noteRepository.GetByCategory(category);
        }

        public async Task<Note> GetById(int id)
        {
            var note = await _noteRepository.Get(id);
            if (note == null)
            {
                throw new NotFoundErrorException("Note not found");
            }
            return note;
        }

        public async Task<List<Note>> GetByUser(string username)
        {
            var user = await _userRepository.Get(username);
            if (user == null)
            {
                throw new NotFoundErrorException("User not found");
            }
            var notes = await _noteRepository.GetAll();
            return notes.Where(note => note.User != null && note.User.Username == username).ToList();
        }
    }
}
