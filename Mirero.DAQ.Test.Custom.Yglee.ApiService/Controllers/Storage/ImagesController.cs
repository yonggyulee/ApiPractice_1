using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Services;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Controllers.Storage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IImageManagementService _imgService;
        // private readonly DatasetDbContext context;
        //
        // public ImagesController(DatasetDbContext context)
        // {
        //     this.context = context;
        // }

        public ImagesController(IMapper mapper, IImageManagementService _imgService)
        {
            _mapper = mapper;
            this._imgService = _imgService;
        }

        // GET: api/Images/datasetId
        [HttpGet("{datasetId}")]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetImages(string datasetId)
        {
            var imgList = await _imgService.ToListAsync(datasetId);
            return imgList.Select(i => _mapper.Map<ImageDTO>(i)).ToList();
        }

        // GET: api/Images/datasetId/5
        [HttpGet("{datasetId}/{id}")]
        public async Task<ActionResult<ImageDTO>> GetImage(string datasetId, string id)
        {
            var image = await _imgService.FindAsync(datasetId, id);

            if (image == null)
            {
                return NotFound();
            }

            return _mapper.Map<ImageDTO>(image);
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
            return File(bytes, "application/octet-stream"); ;
            
        }

        // PUT: api/Images/datasetId/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{datasetId}/{id}")]
        public async Task<IActionResult> PutImage(string datasetId, string id, ImageDTO imageDto)
        {
            if (id != imageDto.ID)
            {
                return BadRequest();
            }

            var image = _mapper.Map<Image>(imageDto);

            var result = await _imgService.UpdateAsync(datasetId, id, image);

            return result switch
            {
                0 => NotFound(),
                -1 => Conflict($"An error occurred while updating the Image.({image.ID})"),
                _ => Content($"Image is updated.({image.ID})")
            };
        }

        // POST: api/Images/datasetId
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{datasetId}")]
        public async Task<ActionResult<ImageDTO>> PostImage(string datasetId, ImageDTO imageDto)
        {
            var image = _mapper.Map<Image>(imageDto);

            image = await _imgService.AddAsync(datasetId, image);

            if (image == null)
            {
                return NotFound($"SampleID doesn't exist.({imageDto.SampleID})");
            }

            return CreatedAtAction("GetImage", new { datasetId = datasetId, id = image.ID }, imageDto);
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
            var image = await _imgService.RemoveAsync(datasetId, id);
            if (image == null)
            {
                return NotFound();
            }
            
            return CreatedAtAction("GetImage", new { datasetId = datasetId, id = image.ID }, _mapper.Map<ImageDTO>(image));
        }
    }
}