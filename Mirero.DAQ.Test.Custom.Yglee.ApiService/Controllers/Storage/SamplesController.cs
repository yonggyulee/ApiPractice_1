using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Storage
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {
        private readonly IMapper _mapper;

        // private readonly DatasetDbContext context;
        //
        // public SamplesController(DatasetDbContext context)
        // {
        //     this.context = context;
        // }
        public SamplesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/Samples/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<SampleDTO>>> GetSamples(string datasetId)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            return await context.Samples
                .Select(s => _mapper.Map<SampleDTO>(s))
                .ToListAsync();
        }

        // GET: api/Samples/datasetId/5
        [HttpGet("{datasetId}/{id}")]
        public async Task<ActionResult<SampleDTO>> GetSample(string datasetId, string id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var sample = await context.Samples.FindAsync(id);

            if (sample == null)
            {
                return NotFound();
            }

            return _mapper.Map<SampleDTO>(sample);
        }

        // PUT: api/Samples/datasetId/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutSample(string datasetId, string id, SampleDTO sampleDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);
            
            if (id != sampleDto.ID)
            {
                return BadRequest();
            }

            var sample = _mapper.Map<Sample>(sampleDto);
            
            context.Entry(sample).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SampleExists(context, id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"Sample is updated.({sample.ID})");
        }

        // POST: api/Samples/datasetId
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{datasetId}")]
        public async Task<ActionResult<SampleDTO>> PostSample(string datasetId, SampleDTO sampleDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var sample = _mapper.Map<Sample>(sampleDto);
            
            context.Samples.Add(sample);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetSample", new { datasetId = datasetId, id = sample.ID }, sampleDto);
        }

        // DELETE: api/Samples/datasetId/5
        [HttpDelete("{datasetId}/{id}")]
        public async Task<IActionResult> DeleteSample(string datasetId, string id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);
            var sample = await context.Samples.FindAsync(id);
            if (sample == null)
            {
                return NotFound();
            }

            context.Samples.Remove(sample);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetSample", new { datasetId = datasetId, id = sample.ID }, _mapper.Map<SampleDTO>(sample));
        }

        private bool SampleExists(DatasetDbContext context, string id)
        {
            return context.Samples.Any(e => e.ID == id);
        }
    }
}