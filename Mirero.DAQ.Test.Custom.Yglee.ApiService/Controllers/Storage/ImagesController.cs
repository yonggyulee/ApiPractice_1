using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Services;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Storage
{
    // TODO  API의 버전은 어떻게 관리하게 되는가.(예: /api/v1/images )
    // TODO  Controller 클래스 명칭으로 부터 api endpoint가 정해지는 것이 정말 유익한지 고민 
    //       (ref : https://code-maze.com/aspnetcore-webapi-best-practices/#routing)
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageManagementService _imgService;
        // private readonly DatasetDbContext context;
        //
        // public ImagesController(DatasetDbContext context)
        // {
        //     this.context = context;
        // }

        public ImagesController(IImageManagementService _imgService)
        {
            this._imgService = _imgService;
        }

        // GET: api/Images/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetImages(string datasetId)
        {
            return await _imgService.ToListAsync(datasetId);
        }

        // GET: api/Images/datasetId/5
        [HttpGet("{datasetId}/{id}")]
        public async Task<ActionResult<ImageDTO>> GetImage(string datasetId, string id)
        {
            var imageDto = await _imgService.FindAsync(datasetId, id);

            if (imageDto == null)
            {
                return NotFound();
            }

            return imageDto;
        }
        
        // GET: api/Images/image_file/datasetId/5
        // 이미지 파일 다운로드
        [HttpGet("image_file/{datasetId}/{id}")]
        public async Task<IActionResult> GetImageFile(string datasetId, string id)
        {
            var bytes = await _imgService.GetImageFileAsync(datasetId, id);

            if (bytes == null)
            {
                return NotFound();
            }

            // 파일 반환
            // TODO Content-Type 이 확인 (예: 이미지 타입)
            // TODO FileStreamResult() 를 사용하여 성능을 향상시킬 수 없는가?(예: 현재 구현은 Full Buffering)
            return File(bytes, "application/octet-stream"); 
            
        }

        // PUT: api/Images/datasetId/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutImage(string datasetId, string id, ImageDTO imageDto)
        {
            if (id != imageDto.Id)
            {
                return BadRequest();
            }
            var result = await _imgService.UpdateAsync(datasetId, id, imageDto);

            return result switch
            {
                0 => NotFound(),
                -1 => Conflict($"An error occurred while updating the Image.({imageDto.Id})"),
                _ => Content($"Image is updated.({imageDto.Id})")
            };
        }

        // POST: api/Images/datasetId
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{datasetId}")]
        public async Task<ActionResult<ImageDTO>> PostImage(string datasetId, ImageDTO imageDto)
        {
            imageDto = await _imgService.AddAsync(datasetId, imageDto);

            if (imageDto == null)
            {
                return NotFound($"SampleID doesn't exist.({imageDto.SampleId})");
            }

            return CreatedAtAction("GetImage", new { datasetId = datasetId, id = imageDto.Id }, imageDto);
        }
        
        // POST: api/Images/upload/datasetId/5
        // 이미지 파일 업로드
        [HttpPost("upload/{datasetId}/{id}")]
        public async Task<Object> UploadImageFile(string datasetId, string id, IFormFile imageFile)
        {            
            if (imageFile == null)
            {
                Console.WriteLine("ImageFile is null.");
                return NotFound("imageFile is null.");
            }

            try
            {
                // 파일 저장
                _imgService.Upload(datasetId, id, imageFile);
            }
            catch (IOException)
            {
                Conflict();
            }

            return Content("Success");
        }

        // DELETE: api/Images/datasetId/5
        [HttpDelete("{datasetId}/{id}")]
        public async Task<IActionResult> DeleteImage(string datasetId, string id)
        {
            var imageDto = await _imgService.RemoveAsync(datasetId, id);
            if (imageDto == null)
            {
                return NotFound();
            }
            
            return CreatedAtAction("GetImage", new { datasetId = datasetId, id = imageDto.Id }, imageDto);
        }
    }
}