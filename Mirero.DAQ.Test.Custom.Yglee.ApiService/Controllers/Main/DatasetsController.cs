using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Services;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatasetsController : ControllerBase
    {
        private readonly IDatasetManagementService _dsService;

        public DatasetsController(IDatasetManagementService dsService)
        {
            _dsService = dsService;
        }

        // GET: api/Datasets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatasetDTO>>> GetDatasets()
        {
            Console.WriteLine(Request.GetDisplayUrl());

            Console.WriteLine(Request.Path.Value);

            return await _dsService.ToListAsync();
        }

        // GET: api/Datasets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DatasetDTO>> GetDataset(int id)
        {
            var datasetDto = await _dsService.FindAsync(id);
            
            if (datasetDto == null)
            {
                return NotFound();
            }
            return datasetDto;
        }

        // PUT: api/Datasets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDataset(int id, DatasetDTO datasetDto)
        {
            if (id != datasetDto.Id)
            {
                return BadRequest();
            }

            var result = await _dsService.UpdateDataset(id, datasetDto);

            return result switch
            {
                0 => NotFound(),
                -1 => Conflict($"An error occurred while updating the Dataset.({datasetDto.Id})"),
                _ => Content($"Dataset is updated.({datasetDto.Id})")
            };
        }

        // POST: api/Datasets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DatasetDTO>> PostDataset(DatasetDTO datasetDto)
        {
            try
            {
                datasetDto = await _dsService.CreateDatasetAsync(datasetDto);

                return CreatedAtAction("GetDataset", new { id = datasetDto.Id }, datasetDto);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return Conflict(e);
            }
        }

        // DELETE: api/Datasets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDataset(int id)
        {
            var datasetDto = await _dsService.RemoveDatasetAsync(id);

            if (datasetDto == null)
            {
                return NotFound();
            }

            return CreatedAtAction("GetDataset", new { id = datasetDto.Id }, datasetDto.Adapt<DatasetDTO>());
        }
    }
}
