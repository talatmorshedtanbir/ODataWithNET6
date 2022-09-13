using ODataWithNET6.Entities;

namespace ODataWithNET6.DataAccess.Abstract
{
    public interface INotesRepository
    {
        IQueryable<Note> GetAll();
        Task<Note> GetByIdAsync(Guid id);
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(Note note);
        Task<bool> NoteExistsAsync(Guid id);
    }
}
