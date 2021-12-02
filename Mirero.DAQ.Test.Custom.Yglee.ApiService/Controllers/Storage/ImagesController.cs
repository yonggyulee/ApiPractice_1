using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;
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
            await using var context = DatasetDbContext.GetInstance(datasetId);
            if (id != imageDto.ID)
            {
                return BadRequest();
            }

            var image = _mapper.Map<Image>(imageDto);
            
            context.Entry(image).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(context, id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content($"Image is updated.({image.ID})");
        }

        // POST: api/Images/datasetId
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{datasetId}")]
        public async Task<ActionResult<ImageDTO>> PostImage(string datasetId, ImageDTO imageDto)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var image = _mapper.Map<Image>(imageDto);
            
            context.Images.Add(image);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetImage", new { datasetId = datasetId, id = image.ID }, imageDto);
        }
        
        // POST: api/Images/upload/datasetId/5
        // 이미지 파일 업로드
        [HttpPost("upload/{datasetId}/{id}")]
        public async Task<Object> UploadImageFile(string datasetId, string id, IFormFile imageFile)
        {
            var verifiedId = IdToVerifiedId(id);
            Console.WriteLine($"UPLOADIMAGEFILE : {verifiedId}");
            
            if (imageFile == null)
            {
                Console.WriteLine("ImageFile is null.");
                return NotFound("imageFile is null.");
            }

            if (imageFile != null)
            {
                Console.WriteLine($"IMAGEFILE SAVE : {verifiedId}");
                // 이미지 파일을 저장할 폴더 경로
                string currentPath = 
                    Path.Combine(Environment.CurrentDirectory, "database", datasetId, "images");

                var verifiedPath = PathToVerifiedPath(Path.Combine(currentPath, verifiedId));
                
                try
                {
                    // 파일 저장
                    SaveFile(imageFile, verifiedPath);
                }
                catch (IOException)
                {
                    Conflict();
                }
            }
            
            return Content("Success");
        }

        // DELETE: api/Images/datasetId/5
        [HttpDelete("{datasetId}/{id}")]
        public async Task<IActionResult> DeleteImage(string datasetId, string id)
        {
            await using var context = DatasetDbContext.GetInstance(datasetId);

            var image = await context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var sample = await context.Samples.FindAsync(image.SampleID);
            if (sample != null) sample.ImageCount -= 1;

            context.Images.Remove(image);
            
            string currentPath = 
                Path.Combine(Environment.CurrentDirectory, image.Path);

            var verifiedPath = PathToVerifiedPath(currentPath);

            RemoveFile(verifiedPath);
            
            await context.SaveChangesAsync();
            
            return CreatedAtAction("GetImage", new { datasetId = datasetId, id = image.ID }, _mapper.Map<ImageDTO>(image));
        }

        private bool ImageExists(DatasetDbContext context, string id)
        {
            return context.Images.Any(e => e.ID == id);
        }
        
        private void SaveFile(IFormFile imgFile, string path)
        {
            // 경로 검증
            Console.WriteLine($"Path : {path}");
            string folderPath = path.Substring(0, path.LastIndexOf('\\'));
            Console.WriteLine($"FolderPath : {folderPath}");

            DirectoryInfo di = new DirectoryInfo(folderPath);
            if (di.Exists == false)
            {
                di.Create();
            }

            if (imgFile.Length > 0)
            {
                using (var imgStream = imgFile.OpenReadStream())
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        imgStream.CopyTo(fileStream);
                    }
                }
            }
        }

        private bool RemoveFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (IOException)
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<IActionResult> GetFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                byte[] bytes;
                using (FileStream file = new FileStream(path, FileMode.Open))
                {
                    try
                    {
                        bytes = new byte[file.Length];
                        await file.ReadAsync(bytes);

                        return File(bytes, "application/octet-stream");
                    }catch(Exception)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }
            return NotFound();
        }

        private string PathToVerifiedPath(string path)
        {
            return path.Replace('/', '\\');
        }

        private string IdToVerifiedId(string id)
        {
            return id.Replace("%2F", "/");
        }
    }
}