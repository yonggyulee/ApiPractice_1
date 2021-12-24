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
    public class JobsController : ControllerBase
    {
        private readonly MainDbContext context;

        public JobsController(MainDbContext context)
        {
            this.context = context;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDTO>>> GetJobs()
        {
            return await context.Jobs
                .Select(j => j.Adapt<JobDTO>()).ToListAsync();
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

            return job.Adapt<JobDTO>();
        }

        // PUT: api/Jobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(int id, JobDTO jobDto)
        {
            if (id != jobDto.Id)
            {
                return BadRequest();
            }

            var job = jobDto.Adapt<Job>();
            
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

            return Content($"Job is updated.({job.Id})");
        }

        // POST: api/Jobs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobDTO>> PostJob(JobDTO jobDto)
        {
            var job = jobDto.Adapt<Job>();
            
            context.Jobs.Add(job);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetJob", new { id = job.Id }, jobDto);
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

            return CreatedAtAction("GetJob", new { id = job.Id }, job.Adapt<JobDTO>());
        }

        private bool JobExists(int id)
        {
            return context.Jobs.Any(e => e.Id == id);
        }
    }
}