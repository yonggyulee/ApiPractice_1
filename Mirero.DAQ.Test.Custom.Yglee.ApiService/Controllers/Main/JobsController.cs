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
    public class JobsController : ControllerBase
    {
        private readonly MainDbContext context;
        private readonly IMapper _mapper;

        public JobsController(MainDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJobs()
        {
            return await context.Jobs
                .Select(j => _mapper.Map<JobDTO>(j)).ToListAsync();
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobDTO>> GetJob(int id)
        {
            var job = await context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return _mapper.Map<JobDTO>(job);
        }

        // PUT: api/Jobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(int id, JobDTO jobDto)
        {
            if (id != jobDto.ID)
            {
                return BadRequest();
            }

            var job = _mapper.Map<Job>(jobDto);
            
            context.Entry(job).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"Job is updated.({job.ID})");
        }

        // POST: api/Jobs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobDTO>> PostJob(JobDTO jobDto)
        {
            var job = _mapper.Map<Job>(jobDto);
            
            context.Jobs.Add(job);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetJob", new { id = job.ID }, jobDto);
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(string id)
        {
            var job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            context.Jobs.Remove(job);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetJob", new { id = job.ID }, _mapper.Map<JobDTO>(job));
        }

        private bool JobExists(int id)
        {
            return context.Jobs.Any(e => e.ID == id);
        }
    }
}