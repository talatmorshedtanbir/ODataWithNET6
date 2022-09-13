using ODataWithNET6.Entities;

namespace ODataWithNET6.Services.Abstract
{
    public interface INotesService
    {
        Task<IList<Note>> GetAllAsync();
        Task<Note> GetByIdAsync(Guid key);
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(Guid key);
        Task<bool> NoteExistsAsync(Guid key);
    }
}
