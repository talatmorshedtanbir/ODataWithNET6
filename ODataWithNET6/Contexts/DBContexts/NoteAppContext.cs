using Microsoft.EntityFrameworkCore;
using ODataWithNET6.Entities;

namespace ODataWithNET6.Contexts.DBContexts
{
    public class NoteAppContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Student> Students { get; set; }

        public NoteAppContext(DbContextOptions<NoteAppContext> options) : base(options)
        {

        }
    }
}
