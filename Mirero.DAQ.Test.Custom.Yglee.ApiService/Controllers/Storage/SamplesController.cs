using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
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
        private readonly MainDbContext _mainDbContext;

        // private readonly DatasetDbContext context;
        //
        // public SamplesController(DatasetDbContext context)
        // {
        //     this.context = context;
        // }
        public SamplesController(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        // GET: api/Samples/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<SampleDTO>>> GetSamples(string datasetId)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            return await context.Samples
                .Select(s => s.Adapt<SampleDTO>())
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

            return sample.Adapt<SampleDTO>();
        }

        // PUT: api/Samples/datasetId/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutSample(string datasetId, string id, SampleDTO sampleDto)
        {
            if (id != sampleDto.Id)
            {
                return BadRequest();
            }

            if (!await VerifyDatasetId(datasetId, sampleDto.DatasetId))
            {
                return BadRequest($"Dataset ID does not match DatasetID.(DatasetID:{sampleDto.DatasetId})");
            }

            await using var context = DatasetDbContext.GetInstance(datasetId);
            
            var sample = sampleDto.Adapt<Sample>();
            
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

            return Content($"Sample is updated.({sample.Id})");
        }

        // POST: api/Samples/datasetId
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{datasetId}")]
        public async Task<ActionResult<SampleDTO>> PostSample(string datasetId, SampleDTO sampleDto)
        {
            if (!await VerifyDatasetId(datasetId, sampleDto.DatasetId))
            {
                return BadRequest($"Dataset ID does not match DatasetID.(DatasetID:{sampleDto.DatasetId})");
            }

            await using var context = DatasetDbContext.GetInstance(datasetId);

            var sample = sampleDto.Adapt<Sample>();
            
            context.Samples.Add(sample);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetSample", new { datasetId = datasetId, id = sample.Id }, sampleDto);
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

            return CreatedAtAction("GetSample", new { datasetId = datasetId, id = sample.Id }, sample.Adapt<SampleDTO>());
        }

        private bool SampleExists(DatasetDbContext context, string id)
        {
            return context.Samples.Any(e => e.Id == id);
        }

        private async Task<bool> VerifyDatasetId(string dsUri, int dsId)
        {
            var dataset = await _mainDbContext.Datasets.FirstAsync(d => d.Uri == dsUri);
            return dataset.Id == dsId;
        }
    }
}