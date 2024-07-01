using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using AutoCADApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AutoCADApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageFilesController : ControllerBase
    {
        private readonly AutoCadContext _context;
        private readonly ILogger<ImageFilesController> _logger;

        public ImageFilesController(AutoCadContext context, ILogger<ImageFilesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageFile>>> GetImageFiles()
        {
            _logger.LogInformation("Fetching all image files with pins.");

            var imageFiles = await _context.ImageFiles.Include(f => f.Pins).ToListAsync();

            if (imageFiles == null || imageFiles.Count == 0)
            {
                _logger.LogWarning("No image files found.");
                return NotFound("No image files found.");
            }

            _logger.LogInformation($"{imageFiles.Count} image files found.");
            return imageFiles;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageFile>> GetImageFile(int id)
        {
            _logger.LogInformation($"Fetching image file with ID {id}.");

            var file = await _context.ImageFiles.Include(f => f.Pins)
                                                .ThenInclude(p => p.ModalContent)
                                                .Include(f => f.Pins)
                                                .ThenInclude(p => p.UploadFile)
                                                .FirstOrDefaultAsync(f => f.Id == id);

            if (file == null)
            {
                _logger.LogWarning($"Image file with ID {id} not found.");
                return NotFound();
            }

            // Convert the file data to a base64 string
            var base64FileData = Convert.ToBase64String(file.FileData);

            // Transform the pins collection to a simple array
            var pinsArray = file.Pins.Select(p => new
            {
                p.Id,
                p.X,
                p.Y,
                p.Status,
                p.Description,
                UploadFile = p.UploadFile == null ? null : new
                {
                    p.UploadFile.Id,
                    p.UploadFile.FileName,
                    FileData = Convert.ToBase64String(p.UploadFile.FileData)
                },
                ModalContent = p.ModalContent == null ? new
                {
                    Id = 0,
                    AdditionalInfo = "",
                    CreatedBy = "",
                    CreatedAt = DateTime.MinValue,
                    UpdatedAt = DateTime.MinValue
                } : new
                {
                    p.ModalContent.Id,
                    p.ModalContent.AdditionalInfo,
                    p.ModalContent.CreatedBy,
                    p.ModalContent.CreatedAt,
                    p.ModalContent.UpdatedAt
                },
                AudioClip = p.AudioClip != null ? Convert.ToBase64String(p.AudioClip) : null
            }).ToArray();

            var response = new
            {
                file.Id,
                file.FileName,
                FileData = base64FileData,
                file.Urn,
                Pins = pinsArray
            };

            _logger.LogInformation($"Returning image file with ID {id} and {pinsArray.Length} pins.");

            return Ok(response);
        }

        [HttpPost("convert")]
        public async Task<ActionResult<string>> ConvertDWGToImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded.");
                return BadRequest("No file uploaded.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var dwgFileData = memoryStream.ToArray();

            // Convert DWG to image using Aspose.CAD
            var imageFileData = ConvertDWGToImage(dwgFileData);

            if (imageFileData == null)
            {
                _logger.LogWarning("Conversion failed.");
                return BadRequest("Conversion failed.");
            }

            var base64Image = Convert.ToBase64String(imageFileData);

            // Log the Base64 image data
            _logger.LogInformation($"Base64 Image Data: {base64Image}");

            return Ok(base64Image);
        }

        [HttpPost]
        public async Task<ActionResult<ImageFile>> PostImageFile([FromForm] IFormFile file, [FromForm] string base64Image = null)
        {
            if (file == null && string.IsNullOrEmpty(base64Image))
            {
                _logger.LogWarning("No file or image data provided.");
                return BadRequest("No file or image data provided.");
            }

            byte[] fileData;

            if (file != null)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }
            else
            {
                fileData = Convert.FromBase64String(base64Image);
            }

            var imageFile = new ImageFile
            {
                FileName = file?.FileName ?? "converted_image.jpg",
                FileData = fileData
            };

            _context.ImageFiles.Add(imageFile);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Image file with ID {imageFile.Id} uploaded successfully.");
            return CreatedAtAction(nameof(GetImageFile), new { id = imageFile.Id }, imageFile);
        }

        private byte[] ConvertDWGToImage(byte[] dwgData)
        {
            using (var stream = new MemoryStream(dwgData))
            {
                var image = Aspose.CAD.Image.Load(stream);

                // Create an instance of CadRasterizationOptions
                var rasterizationOptions = new Aspose.CAD.ImageOptions.CadRasterizationOptions
                {
                    PageWidth = 1200,
                    PageHeight = 1200,
                    BackgroundColor = Aspose.CAD.Color.White,
                    DrawType = Aspose.CAD.FileFormats.Cad.CadDrawTypeMode.UseObjectColor
                };

                // Create an instance of JpegOptions for the converted Jpeg image
                var options = new Aspose.CAD.ImageOptions.JpegOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };

                using (var output = new MemoryStream())
                {
                    // Save DWG to JPEG image
                    image.Save(output, options);
                    return output.ToArray();
                }
            }
        }


    }
}
