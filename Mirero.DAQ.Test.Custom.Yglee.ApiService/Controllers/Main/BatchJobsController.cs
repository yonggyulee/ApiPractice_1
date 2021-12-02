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
    public class BatchJobsController : ControllerBase
    {
        private readonly MainDbContext context;
        private readonly IMapper _mapper;

        public BatchJobsController(MainDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        // GET: api/BatchJobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BatchJobDTO>>> GetBatchJobs()
        {
            return await context.BatchJobs
                .Select(b => _mapper.Map<BatchJobDTO>(b))
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

            return _mapper.Map<BatchJobDTO>(batchjob);
        }

        // PUT: api/BatchJobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBatchJob(int id, BatchJobDTO batchjobDto)
        {
            if (id != batchjobDto.ID)
            {
                return BadRequest();
            }

            var batchjob = _mapper.Map<BatchJob>(batchjobDto);


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

            return Content($"BatchJob is updated.({batchjob.ID})");
        }

        // POST: api/BatchJobs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BatchJobDTO>> PostBatchJob(BatchJobDTO batchJobDto)
        {
            var batchjob = _mapper.Map<BatchJob>(batchJobDto);
            
            context.BatchJobs.Add(batchjob);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetBatchJob", new { id = batchjob.ID }, batchJobDto);
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

            return CreatedAtAction("GetBatchJob", new { id = batchjob.ID }, _mapper.Map<BatchJobDTO>(batchjob));
        }

        private bool BatchJobExists(int id)
        {
            return context.BatchJobs.Any(e => e.ID == id);
        }
    }
}