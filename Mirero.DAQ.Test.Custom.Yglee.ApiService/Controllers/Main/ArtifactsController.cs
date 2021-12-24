using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
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

        public ArtifactsController(MainDbContext context)
        {
            this.context = context;
        }

        // GET: api/Artifacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtifactDTO>>> GetArtifacts()
        {
            return await context.Artifacts
                .Select(a => a.Adapt<ArtifactDTO>())
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

            return artifact.Adapt<ArtifactDTO>();
        }

        // PUT: api/Artifacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtifact(int id, ArtifactDTO artifactDto)
        {
            if (id != artifactDto.Id)
            {
                return BadRequest();
            }

            var artifact = artifactDto.Adapt<Artifact>();
            
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

            return Content($"Artifact is updated.({artifact.Id})");
        }

        // POST: api/Artifacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArtifactDTO>> PostArtifact(ArtifactDTO artifactDto)
        {
            var artifact = artifactDto.Adapt<Artifact>();
            
            context.Artifacts.Add(artifact);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetArtifact", new { id = artifact.Id }, artifactDto);
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

            return CreatedAtAction("GetArtifact", new { id = artifact.Id }, artifact.Adapt<ArtifactDTO>());
        }

        private bool ArtifactExists(int id)
        {
            return context.Artifacts.Any(e => e.Id == id);
        }
    }
}