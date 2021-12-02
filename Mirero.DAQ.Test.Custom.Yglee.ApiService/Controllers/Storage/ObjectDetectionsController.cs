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
    public class ObjectDetectionsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ObjectDetectionsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/ObjectDetections/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<ObjectDetectionLabelDTO>>> GetObjectDetectionLabels(string datasetId)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            return await context.ObjectDetectionLabels
                .Select(o => _mapper.Map<ObjectDetectionLabelDTO>(o)).ToListAsync();
        }
        
        // GET: api/ObjectDetections/datasetId/5
        [HttpGet("{datasetId}/{id}")]
        public async Task<ActionResult<ObjectDetectionLabelDTO>> GetObjectDetectionLabel(string datasetId, int id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var label = await context.ObjectDetectionLabels.FindAsync(id);

            if (label == null)
            {
                return NotFound();
            }

            return _mapper.Map<ObjectDetectionLabelDTO>(label);
        }
        
        // PUT: api/ObjectDetections/datasetId/5
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutObjectDetectionLabel(string datasetId, int id, ObjectDetectionLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);
            if (id != labelDto.ID)
            {
                return BadRequest();
            }

            var label = _mapper.Map<ObjectDetectionLabel>(labelDto);
            
            context.Entry(label).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjectDetectionExists(context, id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"ObjectDetectionLabel is updated.({label.ID})");
        }
        
        // POST: api/ObjectDetections/datasetId
        [HttpPost("{datasetId}")]
        public async Task<IActionResult> PostObjectDetectionLabel(string datasetId, ObjectDetectionLabelDTO labelDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var img = await context.Images.FindAsync(labelDto.ImageID);
            if (img == null)
            {
                return NotFound();
            }

            var label = _mapper.Map<ObjectDetectionLabel>(labelDto);

            if (img.ObjectDetectionLabels != null) img.ObjectDetectionLabels.Add(label);

            await context.SaveChangesAsync();
            
            return CreatedAtAction("GetObjectDetectionLabel", new { datasetId = datasetId, id = label.ID }, labelDto);
        }
        
        // DELETE: api/ObjectDetections/datasetId/6
        [HttpDelete("{datasetId}/{id}")]
        public async Task<IActionResult> DeleteObjectDetectionLabel(string datasetId, int id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var label = await context.ObjectDetectionLabels.FindAsync(id);
            if (label == null)
            {
                return NotFound();
            }
            
            context.ObjectDetectionLabels.Remove(label);
            
            await context.SaveChangesAsync();
            
            return CreatedAtAction("GetObjectDetectionLabel", new { datasetId = datasetId, id = label.ID }, _mapper.Map<ObjectDetectionLabelDTO>(label));
        }
        
        private bool ObjectDetectionExists(DatasetDbContext context, int id)
        {
            return context.ObjectDetectionLabels.Any(e => e.ID == id);
        }
    }
}