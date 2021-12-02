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
    public class LabelSetsController : ControllerBase
    {
        private readonly IMapper _mapper;
        // private readonly DatasetDbContext context;
        //
        // public LabelSetsController(DatasetDbContext context)
        // {
        //     this.context = context;
        // }

        public LabelSetsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/LabelSets/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<LabelSetDTO>>> GetLabelSets(string datasetId)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            return await context.LabelSets
                .Select(l => _mapper.Map<LabelSetDTO>(l))
                .ToListAsync();
        }

        // GET: api/LabelSets/datasetId/5
        [HttpGet("{datasetId}/{id}")]
        public async Task<ActionResult<LabelSetDTO>> GetLabelSet(string datasetId, string id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var labelset = await context.LabelSets.FindAsync(id);

            if (labelset == null)
            {
                return NotFound();
            }

            return _mapper.Map<LabelSetDTO>(labelset);
        }

        // PUT: api/LabelSets/datasetId/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutLabelSet(string datasetId, string id, LabelSetDTO labelSetDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);
            if (id != labelSetDto.ID)
            {
                return BadRequest();
            }

            var labelset = _mapper.Map<LabelSet>(labelSetDto);
            
            context.Entry(labelset).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabelSetExists(context, id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"LabelSet is updated.({labelset.ID})");
        }

        // POST: api/LabelSets/datasetId
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{datasetId}")]
        public async Task<ActionResult<LabelSetDTO>> PostLabelSet(string datasetId, LabelSetDTO labelSetDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var labelset = _mapper.Map<LabelSet>(labelSetDto);
            
            context.LabelSets.Add(labelset);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetLabelSet", new { datasetId = datasetId, id = labelset.ID }, labelSetDto);
        }

        // DELETE: api/LabelSets/datasetId/5
        [HttpDelete("{datasetId}/{id}")]
        public async Task<IActionResult> DeleteLabelSet(string datasetId, string id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var labelSet = await context.LabelSets.FindAsync(id);
            if (labelSet == null)
            {
                return NotFound();
            }

            context.LabelSets.Remove(labelSet);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetLabelSet", new { datasetId = datasetId, id = labelSet.ID }, _mapper.Map<LabelSetDTO>(labelSet));
        }

        private bool LabelSetExists(DatasetDbContext context, string id)
        {
            return context.LabelSets.Any(e => e.ID == id);
        }
    }
}