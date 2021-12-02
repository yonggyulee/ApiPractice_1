using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;

        public ClassCodeSetsController(MainDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        // GET: api/ClassCodeSets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassCodeSetDTO>>> GetClassCodeSets()
        {
            return await context.ClassCodeSets
                .Select(c => _mapper.Map<ClassCodeSetDTO>(c))
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

            return _mapper.Map<ClassCodeSetDTO>(classcodeset);
        }

        // PUT: api/ClassCodeSets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassCodeSet(string id, ClassCodeSetDTO classCodeSetDto)
        {
            if (id != classCodeSetDto.ID)
            {
                return BadRequest();
            }
            
            // 수정된 시간
            classCodeSetDto.UpdatedAt = DateTime.Now;

            var classcodeset = _mapper.Map<ClassCodeSet>(classCodeSetDto);
            
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

            return Content($"ClassCodeSet is updated.({classcodeset.ID})");
        }

        // POST: api/ClassCodeSets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassCodeSetDTO>> PostClassCodeSet(ClassCodeSetDTO classCodeSetDto)
        {
            var classcodeset = _mapper.Map<ClassCodeSet>(classCodeSetDto);
            
            context.ClassCodeSets.Add(classcodeset);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetClassCodeSet", new { id = classcodeset.ID }, classCodeSetDto);
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

            return CreatedAtAction("GetClassCodeSet", new { id = classcodeset.ID }, _mapper.Map<ClassCodeSetDTO>(classcodeset));
        }

        private bool ClassCodeSetExists(string id)
        {
            return context.ClassCodeSets.Any(e => e.ID == id);
        }
    }
}