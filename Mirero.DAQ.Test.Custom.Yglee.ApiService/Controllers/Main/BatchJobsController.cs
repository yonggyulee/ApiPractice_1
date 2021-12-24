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
    public class BatchJobsController : ControllerBase
    {
        private readonly MainDbContext context;

        public BatchJobsController(MainDbContext context)
        {
            this.context = context;
        }

        // GET: api/BatchJobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BatchJobDTO>>> GetBatchJobs()
        {
            return await context.BatchJobs
                .Select(b => b.Adapt<BatchJobDTO>())
                .ToListAsync();
        }

        // GET: api/BatchJobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BatchJobDTO>> GetBatchJob(int id)
        {
            var batchjob = await context.BatchJobs.FindAsync(id);

            if (batchjob == null)
            {
                return NotFound();
            }

            return batchjob.Adapt<BatchJobDTO>();
        }

        // PUT: api/BatchJobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBatchJob(int id, BatchJobDTO batchjobDto)
        {
            if (id != batchjobDto.Id)
            {
                return BadRequest();
            }

            var batchjob = batchjobDto.Adapt<BatchJob>();


            context.Entry(batchjob).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchJobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"BatchJob is updated.({batchjob.Id})");
        }

        // POST: api/BatchJobs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BatchJobDTO>> PostBatchJob(BatchJobDTO batchJobDto)
        {
            var batchjob = batchJobDto.Adapt<BatchJob>();
            
            context.BatchJobs.Add(batchjob);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetBatchJob", new { id = batchjob.Id }, batchJobDto);
        }

        // DELETE: api/BatchJobs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatchJob(int id)
        {
            var batchjob = await context.BatchJobs.FindAsync(id);
            if (batchjob == null)
            {
                return NotFound();
            }

            context.BatchJobs.Remove(batchjob);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetBatchJob", new { id = batchjob.Id }, batchjob.Adapt<BatchJobDTO>());
        }

        private bool BatchJobExists(int id)
        {
            return context.BatchJobs.Any(e => e.Id == id);
        }
    }
}