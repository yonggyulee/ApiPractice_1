using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ClassCodesController(MainDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        // GET: api/ClassCodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassCodeDTO>>> GetClassCodes()
        {
            return await context.ClassCodes
                .Select(c => _mapper.Map<ClassCodeDTO>(c))
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

            return _mapper.Map<ClassCodeDTO>(classcode);
        }

        // PUT: api/ClassCodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassCode(int id, ClassCodeDTO classCodeDto)
        {
            if (id != classCodeDto.ID)
            {
                return BadRequest();
            }

            var classcode = _mapper.Map<ClassCode>(classCodeDto);
            
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

            return Content($"ClassCode is updated.({classcode.ID})");
        }

        // POST: api/ClassCodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClassCodeDTO>> PostClassCode(ClassCodeDTO classCodeDto)
        {
            var classcode = _mapper.Map<ClassCode>(classCodeDto);
            
            context.ClassCodes.Add(classcode);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetClassCode", new { id = classcode.ID }, classCodeDto);
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

            return CreatedAtAction("GetClassCode", new { id = classcode.ID }, _mapper.Map<ClassCodeDTO>(classcode));
        }

        private bool ClassCodeExists(int id)
        {
            return context.ClassCodes.Any(e => e.ID == id);
        }
    }
}