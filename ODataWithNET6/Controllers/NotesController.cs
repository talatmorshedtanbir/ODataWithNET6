using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using ODataWithNET6.Contexts.DBContexts;
using ODataWithNET6.Entities;

namespace ODataWithNET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteAppContext _context;

        private readonly ILogger<NotesController> _logger;

        public NotesController(NoteAppContext dbContext, ILogger<NotesController> logger)
        {
            _logger = logger;
            _context = dbContext;
        }

        [HttpGet]
        [EnableQuery(PageSize = 15)]
        public IEnumerable<Note> Get()
        {
            return _context.Notes;
        }

        [HttpGet("key")]
        [EnableQuery()]
        public IActionResult Get([FromRoute] Guid key)
        {
            var result = _context.Notes.Where(c => c.Id == key);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { key = note.Id }, note);
        }

        [HttpPut("{key}")]
        public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != note.Id)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("key")]
        public async Task<IActionResult> Delete([FromRoute] Guid key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _context.Notes.FindAsync(key);
            if (student == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        [HttpGet("noteexists/{ket}")]
        private bool NoteExists(Guid key)
        {
            return _context.Notes.Any(p => p.Id == key);
        }
    }
}
