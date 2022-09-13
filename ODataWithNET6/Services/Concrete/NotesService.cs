using Microsoft.EntityFrameworkCore;
using ODataWithNET6.DataAccess.Abstract;
using ODataWithNET6.Entities;
using ODataWithNET6.Services.Abstract;

namespace ODataWithNET6.Services.Concrete
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository notesRepository;

        public NotesService(INotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }

        public async Task AddAsync(Note note)
        {
            await notesRepository.AddAsync(note);
        }

        public async Task DeleteAsync(Guid key)
        {
            var note = await notesRepository.GetByIdAsync(key);

            if (note is null)
            {
                throw new InvalidOperationException();
            }

            await notesRepository.DeleteAsync(note);
        }

        public async Task<IList<Note>> GetAllAsync()
        {
            return await notesRepository.GetAll().ToListAsync();
        }

        public async Task<Note> GetByIdAsync(Guid key)
        {
            return await notesRepository.GetByIdAsync(key);
        }

        public async Task<bool> NoteExistsAsync(Guid key)
        {
            return await notesRepository.NoteExistsAsync(key);
        }

        public async Task UpdateAsync(Note note)
        {
            var existingNote = await notesRepository.GetByIdAsync(note.Id);
            if (existingNote == null)
            {
                throw new Exception();
            }

            existingNote.MessageNote = note.MessageNote;

            await notesRepository.UpdateAsync(existingNote);
        }
    }
}
