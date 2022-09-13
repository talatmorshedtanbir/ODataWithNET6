using Microsoft.EntityFrameworkCore;
using ODataWithNET6.Contexts.DBContexts;
using ODataWithNET6.DataAccess.Abstract;
using ODataWithNET6.Entities;

namespace ODataWithNET6.DataAccess.Concrete
{
    public class NotesRepository : INotesRepository
    {
        private readonly NoteAppContext context;

        public NotesRepository(NoteAppContext context)
        {
            this.context = context;
        }

        public IQueryable<Note> GetAll() => context.Notes.AsQueryable();

        public async Task<Note> GetByIdAsync(Guid id) => await context.Notes.SingleOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(Note note)
        {
            await context.Notes.AddAsync(note);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note note)
        {
            context.Notes.Update(note);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Note note)
        {
            context.Notes.Remove(note);
            await context.SaveChangesAsync();
        }

        public async Task<bool> NoteExistsAsync(Guid id) => await context.Notes.AnyAsync(x => x.Id == id);
    }
}
