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
    public class SegmentationsController : ControllerBase
    {
        public SegmentationsController()
        {
        }

        // GET: api/Segmentations/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<SegmentationLabelDTO>>> GetSegmentationLabels(string datasetId)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            return await context.SegmentationLabels
                .Select(s => s.Adapt<SegmentationLabelDTO>()).ToListAsync();
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

            return label.Adapt<SegmentationLabelDTO>();
        }
        
        // PUT: api/Segmentations/datasetId/5
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutSegmentationLabel(string datasetId, int id, SegmentationLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);
            if (id != labelDto.Id)
            {
                return BadRequest();
            }

            var label = labelDto.Adapt<SegmentationLabel>();
            
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

            return Content($"SegmentationLabel is updated.({label.Id})");
        }
        
        // POST: api/Segmentations/datasetId
        [HttpPost("{datasetId}")]
        public async Task<IActionResult> PostSegmentationLabel(string datasetId, SegmentationLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var img = await context.Images.FindAsync(labelDto.ImageId);
            if (img == null)
            {
                return NotFound();
            }

            var label = labelDto.Adapt<SegmentationLabel>();

            if (img.SegmentationLabels != null) img.SegmentationLabels.Add(label);

            await context.SaveChangesAsync();
            
            return CreatedAtAction("GetSegmentationLabel", new { datasetId = datasetId, id = label.Id }, labelDto);
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
            
            return CreatedAtAction("GetSegmentationLabel", new { datasetId = datasetId, id = label.Id }, label.Adapt<SegmentationLabelDTO>());
        }
        
        private bool SegmentationExists(DatasetDbContext context, int id)
        {
            return context.SegmentationLabels.Any(e => e.Id == id);
        }
    }
}