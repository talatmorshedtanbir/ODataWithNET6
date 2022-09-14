﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ODataWithNET6.Contexts.DBContexts;
using ODataWithNET6.Entities;

namespace ODataWithNET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ODataController
    {
        private readonly NoteAppContext _db;

        private readonly ILogger<NotesController> _logger;

        public NotesController(NoteAppContext dbContext, ILogger<NotesController> logger)
        {
            _logger = logger;
            _db = dbContext;
        }

        [HttpGet]
        [EnableQuery(PageSize = 15)]
        public IQueryable<Note> Get()
        {
            return _db.Notes;
        }

        [HttpGet("key")]
        [EnableQuery()]
        public SingleResult<Note> Get([FromODataUri] Guid key)
        {
            var result = _db.Notes.Where(c => c.Id == key);
            return SingleResult.Create(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Note note)
        {
            _db.Notes.Add(note);
            await _db.SaveChangesAsync();
            return Created(note);
        }

        [HttpPut("{key}")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Note> note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingNote = await _db.Notes.FindAsync(key);
            if (existingNote == null)
            {
                return NotFound();
            }

            note.Patch(existingNote);
            try
            {
                await _db.SaveChangesAsync();
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
            return Updated(existingNote);
        }

        [HttpDelete("key")]
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Note existingNote = await _db.Notes.FindAsync(key);
            if (existingNote == null)
            {
                return NotFound();
            }

            _db.Notes.Remove(existingNote);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpGet("noteexists/{ket}")]
        private bool NoteExists(Guid key)
        {
            return _db.Notes.Any(p => p.Id == key);
        }
    }
}
