using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public DatasetsController(IDatasetManagementService dsService, IMapper mapper)
        {
            _dsService = dsService;
            _mapper = mapper;
        }

        // GET: api/Datasets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatasetDTO>>> GetDatasets()
        {
            var dsList = await _dsService.ToListAsync();
            return dsList.Select(d => _mapper.Map<DatasetDTO>(d)).ToList();
        }

        // GET: api/Datasets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DatasetDTO>> GetDataset(int id)
        {
            var dataset = await _dsService.FindAsync(id);
            
            if (dataset == null)
            {
                return NotFound();
            }
            return _mapper.Map<DatasetDTO>(dataset);
        }

        // PUT: api/Datasets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDataset(int id, DatasetDTO datasetDto)
        {
            if (id != datasetDto.ID)
            {
                return BadRequest();
            }

            var dataset = _mapper.Map<Dataset>(datasetDto);

            var result = await _dsService.UpdateDataset(id, dataset);

            return result switch
            {
                0 => NotFound(),
                -1 => Conflict($"An error occurred while updating the Dataset.({dataset.ID})"),
                _ => Content($"Dataset is updated.({dataset.ID})")
            };
        }

        // POST: api/Datasets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DatasetDTO>> PostDataset(DatasetDTO datasetDto)
        {
            try
            {
                var dataset = _mapper.Map<Dataset>(datasetDto);
                dataset = await _dsService.CreateDatasetAsync(dataset);

                return CreatedAtAction("GetDataset", new { id = datasetDto.ID }, _mapper.Map<DatasetDTO>(dataset));
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
            var dataset = await _dsService.RemoveDatasetAsync(id);

            if (dataset == null)
            {
                return NotFound();
            }

            return CreatedAtAction("GetDataset", new { id = dataset.ID }, _mapper.Map<DatasetDTO>(dataset));
        }
    }
}
