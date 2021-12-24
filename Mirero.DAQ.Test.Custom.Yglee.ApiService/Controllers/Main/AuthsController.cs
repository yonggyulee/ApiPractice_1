using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly MainDbContext context;

        public AuthsController(MainDbContext context)
        {
            this.context = context;
        }

        // GET: api/Auths
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthDTO>>> GetAuths()
        {
            return await context.Auths
                .Select(a => a.Adapt<AuthDTO>())
                .ToListAsync();
        }

        // GET: api/Auths/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthDTO>> GetAuth(string id)
        {
            var auth = await context.Auths.FindAsync(id);

            if (auth == null)
            {
                return NotFound();
            }

            return auth.Adapt<AuthDTO>();
        }

        // PUT: api/Auths/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuth(string id, AuthDTO authDto)
        {
            if (id != authDto.Id)
            {
                return BadRequest();
            }

            var auth = authDto.Adapt<Auth>();
            
            context.Entry(auth).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"Auth is updated.({auth.Id})");
        }

        // POST: api/Auths
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthDTO>> PostAuth(AuthDTO authDto)
        {
            var auth = authDto.Adapt<Auth>();

            context.Auths.Add(auth);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetAuth", new { id = auth.Id }, authDto);
        }

        // DELETE: api/Auths/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuth(string id)
        {
            var auth = await context.Auths.FindAsync(id);
            if (auth == null)
            {
                return NotFound();
            }

            context.Auths.Remove(auth);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetAuth", new { id = auth.Id }, auth.Adapt<AuthDTO>());
        }

        private bool AuthExists(string id)
        { 
            return context.Auths.Any(e => e.Id == id);
        }
    }
}