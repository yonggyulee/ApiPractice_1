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
    public class ClassCodesController : ControllerBase
    {
        private readonly MainDbContext context;

        public ClassCodesController(MainDbContext context)
        {
            this.context = context;
        }

        // GET: api/ClassCodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassCodeDTO>>> GetClassCodes()
        {
            return await context.ClassCodes
                .Select(c => c.Adapt<ClassCodeDTO>())
                .ToListAsync();
            // return await context.ClassCodes.ToListAsync();
        }

        // GET: api/ClassCodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassCodeDTO>> GetClassCode(int id)
        {
            var classcode = await context.ClassCodes.FindAsync(id);

            if (classcode == null)
            {
                return NotFound();
            }

            return classcode.Adapt<ClassCodeDTO>();
        }

        // PUT: api/ClassCodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassCode(int id, ClassCodeDTO classCodeDto)
        {
            if (id != classCodeDto.Id)
            {
                return BadRequest();
            }

            var classcode = classCodeDto.Adapt<ClassCode>();
            
            context.Entry(classcode).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassCodeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"ClassCode is updated.({classcode.Id})");
        }

        // POST: api/ClassCodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassCodeDTO>> PostClassCode(ClassCodeDTO classCodeDto)
        {
            var classcode = classCodeDto.Adapt<ClassCode>();
            
            context.ClassCodes.Add(classcode);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetClassCode", new { id = classcode.Id }, classCodeDto);
        }

        // DELETE: api/ClassCodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassCode(int id)
        {
            var classcode = await context.ClassCodes.FindAsync(id);
            if (classcode == null)
            {
                return NotFound();
            }

            context.ClassCodes.Remove(classcode);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetClassCode", new { id = classcode.Id }, classcode.Adapt<ClassCodeDTO>());
        }

        private bool ClassCodeExists(int id)
        {
            return context.ClassCodes.Any(e => e.Id == id);
        }
    }
}