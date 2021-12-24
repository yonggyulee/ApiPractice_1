using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassCodeSetsController : ControllerBase
    {
        private readonly MainDbContext context;

        public ClassCodeSetsController(MainDbContext context)
        {
            this.context = context;
        }

        // GET: api/ClassCodeSets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassCodeSetDTO>>> GetClassCodeSets()
        {
            return await context.ClassCodeSets
                .Select(c => c.Adapt<ClassCodeSetDTO>())
                .ToListAsync();
        }

        // GET: api/ClassCodeSets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassCodeSetDTO>> GetClassCodeSet(string id)
        {
            var classcodeset = await context.ClassCodeSets.FindAsync(id);

            if (classcodeset == null)
            {
                return NotFound();
            }

            return classcodeset.Adapt<ClassCodeSetDTO>();
        }

        // PUT: api/ClassCodeSets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassCodeSet(string id, ClassCodeSetDTO classCodeSetDto)
        {
            if (id != classCodeSetDto.Id)
            {
                return BadRequest();
            }
            
            // 수정된 시간
            classCodeSetDto.UpdatedAt = DateTime.Now;

            var classcodeset = classCodeSetDto.Adapt<ClassCodeSet>();
            
            context.Entry(classcodeset).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassCodeSetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"ClassCodeSet is updated.({classcodeset.Id})");
        }

        // POST: api/ClassCodeSets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassCodeSetDTO>> PostClassCodeSet(ClassCodeSetDTO classCodeSetDto)
        {
            var classcodeset = classCodeSetDto.Adapt<ClassCodeSet>();
            
            context.ClassCodeSets.Add(classcodeset);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetClassCodeSet", new { id = classcodeset.Id }, classCodeSetDto);
        }

        // DELETE: api/ClassCodeSets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassCodeSet(string id)
        {
            var classcodeset = await context.ClassCodeSets.FindAsync(id);
            if (classcodeset == null)
            {
                return NotFound($"ClassCodeSet(id:{id})");
            }

            context.ClassCodeSets.Remove(classcodeset);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetClassCodeSet", new { id = classcodeset.Id }, classcodeset.Adapt<ClassCodeSetDTO>());
        }

        private bool ClassCodeSetExists(string id)
        {
            return context.ClassCodeSets.Any(e => e.Id == id);
        }
    }
}