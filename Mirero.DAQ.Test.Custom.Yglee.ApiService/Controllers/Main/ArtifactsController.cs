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
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtifactsController : ControllerBase
    {
        private readonly MainDbContext context;
        private readonly IMapper _mapper;

        public ArtifactsController(MainDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        // GET: api/Artifacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtifactDTO>>> GetArtifacts()
        {
            return await context.Artifacts
                .Select(a => _mapper.Map<ArtifactDTO>(a))
                .ToListAsync();
        }

        // GET: api/Artifacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtifactDTO>> GetArtifact(int id)
        {
            var artifact = await context.Artifacts.FindAsync(id);
            
            if (artifact == null)
            {
                return NotFound();
            }
            
            return _mapper.Map<ArtifactDTO>(artifact);
        }

        // PUT: api/Artifacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtifact(int id, ArtifactDTO artifactDto)
        {
            if (id != artifactDto.ID)
            {
                return BadRequest();
            }

            var artifact = _mapper.Map<Artifact>(artifactDto);
            
            context.Entry(artifact).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtifactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"Artifact is updated.({artifact.ID})");
        }

        // POST: api/Artifacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArtifactDTO>> PostArtifact(ArtifactDTO artifactDto)
        {
            var artifact = _mapper.Map<Artifact>(artifactDto);
            
            context.Artifacts.Add(artifact);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetArtifact", new { id = artifact.ID }, artifactDto);
        }

        // DELETE: api/Artifacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtifact(int id)
        {
            var artifact = await context.Artifacts.FindAsync(id);
            if (artifact == null)
            {
                return NotFound();
            }

            context.Artifacts.Remove(artifact);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetArtifact", new { id = artifact.ID }, _mapper.Map<ArtifactDTO>(artifact));
        }

        private bool ArtifactExists(int id)
        {
            return context.Artifacts.Any(e => e.ID == id);
        }
    }
}