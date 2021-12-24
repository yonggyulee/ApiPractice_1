using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Storage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationsController : ControllerBase
    {

        public ClassificationsController()
        {
        }

        // GET: api/Classifications/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<ClassificationLabelDTO>>> GetClassificationLabels(string datasetId)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            return await context.ClassificationLabels
                .Select(c => c.Adapt<ClassificationLabelDTO>())
                .ToListAsync();
        }
        
        // GET: api/Classifications/datasetId/5
        [HttpGet("{datasetId}/{id}")]
        public async Task<ActionResult<ClassificationLabelDTO>> GetClassificationLabel(string datasetId, int id)
        {
            // TODO 아래 구현을  IIMageManagementService 처럼 서비스 내로 이동하면 어떤 점이 좋아지는가
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var label = await context.ClassificationLabels.FindAsync(id);

            if (label == null)
            {
                return NotFound();
            }

            return label.Adapt<ClassificationLabelDTO>();
        }
        
        // PUT: api/Classifications/datasetId/5
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutClassificationLabel(string datasetId, int id, ClassificationLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);
            
            if (id != labelDto.Id)
            {
                return BadRequest();
            }

            var label = labelDto.Adapt<ClassificationLabelDTO>();
            
            context.Entry(label).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassificationExists(context, id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"ClassificationLabel is updated.({label.Id})");
        }
        
        // POST: api/Classifications/datasetId
        [HttpPost("{datasetId}")]
        public async Task<IActionResult> PostClassificationLabel(string datasetId, ClassificationLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var img = await context.Images.FindAsync(labelDto.ImageId);
            
            if (img == null)
            {
                return NotFound();
            }
            
            var label = labelDto.Adapt<ClassificationLabel>();

            if (img.ClassificationLabels != null) img.ClassificationLabels.Add(label);

            await context.SaveChangesAsync();
            
            return CreatedAtAction("GetClassificationLabel", new { datasetId = datasetId, id = label.Id }, label);
        }
        
        // DELETE: api/Classifications/datasetId/6
        [HttpDelete("{datasetId}/{id}")]
        public async Task<IActionResult> DeleteClassificationLabel(string datasetId, int id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var label = await context.ClassificationLabels.FindAsync(id);
            if (label == null)
            {
                return NotFound();
            }
            
            context.ClassificationLabels.Remove(label);
            
            await context.SaveChangesAsync();
            
            return CreatedAtAction("GetClassificationLabel", new { datasetId = datasetId, id = label.Id }, label.Adapt<ClassificationLabelDTO>());
        }
        
        private bool ClassificationExists(DatasetDbContext context, int id)
        {
            return context.ClassificationLabels.Any(e => e.Id == id);
        }
    }
}