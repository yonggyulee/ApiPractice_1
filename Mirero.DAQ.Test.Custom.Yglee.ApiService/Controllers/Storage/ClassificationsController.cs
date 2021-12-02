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
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Storage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ClassificationsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/Classifications/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<ClassificationLabelDTO>>> GetClassificationLabels(string datasetId)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            return await context.ClassificationLabels
                .Select(c => _mapper.Map<ClassificationLabelDTO>(c))
                .ToListAsync();
        }
        
        // GET: api/Classifications/datasetId/5
        [HttpGet("{datasetId}/{id}")]
        public async Task<ActionResult<ClassificationLabelDTO>> GetClassificationLabel(string datasetId, int id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var label = await context.ClassificationLabels.FindAsync(id);

            if (label == null)
            {
                return NotFound();
            }

            return _mapper.Map<ClassificationLabelDTO>(label);
        }
        
        // PUT: api/Classifications/datasetId/5
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutClassificationLabel(string datasetId, int id, ClassificationLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);
            
            if (id != labelDto.ID)
            {
                return BadRequest();
            }

            var label = _mapper.Map<ClassificationLabel>(labelDto);
            
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

            return Content($"ClassificationLabel is updated.({label.ID})");
        }
        
        // POST: api/Classifications/datasetId
        [HttpPost("{datasetId}")]
        public async Task<IActionResult> PostClassificationLabel(string datasetId, ClassificationLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var img = await context.Images.FindAsync(labelDto.ImageID);
            
            if (img == null)
            {
                return NotFound();
            }
            
            var label = _mapper.Map<ClassificationLabel>(labelDto);

            if (img.ClassificationLabels != null) img.ClassificationLabels.Add(label);

            await context.SaveChangesAsync();
            
            return CreatedAtAction("GetClassificationLabel", new { datasetId = datasetId, id = label.ID }, label);
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
            
            return CreatedAtAction("GetClassificationLabel", new { datasetId = datasetId, id = label.ID }, _mapper.Map<ClassificationLabelDTO>(label));
        }
        
        private bool ClassificationExists(DatasetDbContext context, int id)
        {
            return context.ClassificationLabels.Any(e => e.ID == id);
        }
    }
}