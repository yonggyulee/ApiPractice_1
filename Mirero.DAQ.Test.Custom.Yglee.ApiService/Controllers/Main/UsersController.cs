using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MainDbContext context;
        private readonly IMapper _mapper;

        public UsersController(MainDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await context.Users
                .Select(u => _mapper.Map<UserDTO>(u))
                .ToListAsync();

            // return await context.Users
            //     .Include(u => u.UserAuthMaps)
            //     .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserDTO>(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserDTO userDto)
        {
            if (id != userDto.ID)
            {
                return BadRequest();
            }

            var user = _mapper.Map<User>(userDto);
            
            context.Entry(user).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"User is updated.({user.ID})");
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.ID }, userDto);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.ID }, _mapper.Map<UserDTO>(user));
        }

        private bool UserExists(string id)
        {
            return context.Users.Any(e => e.ID == id);
        }
    }
}