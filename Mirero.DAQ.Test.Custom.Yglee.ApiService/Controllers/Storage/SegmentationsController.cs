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
    public class SegmentationsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public SegmentationsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/Segmentations/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<SegmentationLabelDTO>>> GetSegmentationLabels(string datasetId)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            return await context.SegmentationLabels
                .Select(s => _mapper.Map<SegmentationLabelDTO>(s)).ToListAsync();
        }
        
        // GET: api/Segmentations/datasetId/5
        [HttpGet("{datasetId}/{id}")]
        public async Task<ActionResult<SegmentationLabelDTO>> GetSegmentationLabel(string datasetId, int id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var label = await context.SegmentationLabels.FindAsync(id);

            if (label == null)
            {
                return NotFound();
            }

            return _mapper.Map<SegmentationLabelDTO>(label);
        }
        
        // PUT: api/Segmentations/datasetId/5
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutSegmentationLabel(string datasetId, int id, SegmentationLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);
            if (id != labelDto.ID)
            {
                return BadRequest();
            }

            var label = _mapper.Map<SegmentationLabel>(labelDto);
            
            context.Entry(label).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SegmentationExists(context, id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"SegmentationLabel is updated.({label.ID})");
        }
        
        // POST: api/Segmentations/datasetId
        [HttpPost("{datasetId}")]
        public async Task<IActionResult> PostSegmentationLabel(string datasetId, SegmentationLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var img = await context.Images.FindAsync(labelDto.ImageID);
            if (img == null)
            {
                return NotFound();
            }

            var label = _mapper.Map<SegmentationLabel>(labelDto);

            if (img.SegmentationLabels != null) img.SegmentationLabels.Add(label);

            await context.SaveChangesAsync();
            
            return CreatedAtAction("GetSegmentationLabel", new { datasetId = datasetId, id = label.ID }, labelDto);
        }
        
        // DELETE: api/Segmentations/datasetId/6
        [HttpDelete("{datasetId}/{id}")]
        public async Task<IActionResult> DeleteSegmentationLabel(string datasetId, int id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var label = await context.SegmentationLabels.FindAsync(id);
            if (label == null)
            {
                return NotFound();
            }
            
            context.SegmentationLabels.Remove(label);
            
            await context.SaveChangesAsync();
            
            return CreatedAtAction("GetSegmentationLabel", new { datasetId = datasetId, id = label.ID }, _mapper.Map<SegmentationLabelDTO>(label));
        }
        
        private bool SegmentationExists(DatasetDbContext context, int id)
        {
            return context.SegmentationLabels.Any(e => e.ID == id);
        }
    }
}