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
    public class AuthsController : ControllerBase
    {
        private readonly MainDbContext context;
        private readonly IMapper _mapper;

        public AuthsController(MainDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        // GET: api/Auths
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthDTO>>> GetAuths()
        {
            return await context.Auths
                .Select(a => _mapper.Map<AuthDTO>(a))
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

            return _mapper.Map<AuthDTO>(auth);
        }

        // PUT: api/Auths/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuth(string id, AuthDTO authDto)
        {
            if (id != authDto.ID)
            {
                return BadRequest();
            }

            var auth = _mapper.Map<Auth>(authDto);
            
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

            return Content($"Auth is updated.({auth.ID})");
        }

        // POST: api/Auths
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthDTO>> PostAuth(AuthDTO authDto)
        {
            var auth = _mapper.Map<Auth>(authDto);

            context.Auths.Add(auth);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetAuth", new { id = auth.ID }, authDto);
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

            return CreatedAtAction("GetAuth", new { id = auth.ID }, _mapper.Map<AuthDTO>(auth));
        }

        private bool AuthExists(string id)
        { 
            return context.Auths.Any(e => e.ID == id);
        }
    }
}