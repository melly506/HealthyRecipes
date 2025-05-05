namespace RecipeManagement.Controllers.v1;


using RecipeManagement.Resources;
using RecipeManagement.Services;
using RecipeManagement.Domain;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Threading;
using Asp.Versioning;
using System.Net;

[ApiController]
[Route("api/v{v:apiVersion}/images")]
[ApiVersion("1.0")]
public sealed class ImageUploadController(IConfiguration configuration) : ControllerBase
{

    private readonly Cloudinary _cloudinary = new Cloudinary(configuration.GetCloudinaryUrl());

    /// <summary>
    /// Uploads an image to Cloudinary.
    /// </summary>
    /// <param name="file">The image file to upload.</param>
    [Authorize]
    [HttpPost("uploadRecipeImage")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file was uploaded.");

        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true,
            Transformation = new Transformation()
                .Width(864)                     // Resize width
                .Height(1080)                   // Resize height
                .Crop("fill")                   // Crop mode: fill to maintain aspect ratio
                .Gravity("auto")                // Automatically focus on the most interesting part
                .Quality("auto:best")           // Auto-optimize quality
                .FetchFormat("auto")            // Auto-select best format (e.g., WebP if supported)
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode == HttpStatusCode.OK)
        {
            return Ok(new
            {
                SecureUrl = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId,
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                Format = uploadResult.Format,
                Bytes = uploadResult.Bytes,
                Version = uploadResult.Version,
                CreatedAt = uploadResult.CreatedAt
            });
        }
        else
        {
            return StatusCode((int)uploadResult.StatusCode, uploadResult.Error?.Message);
        }
    }

    /// <summary>
    /// Uploads a user profile image to Cloudinary, focusing on faces if present.
    /// </summary>
    /// <param name="file">The image file to upload.</param>
    [Authorize]
    [HttpPost("uploadProfileImage")]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file was uploaded.");

        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true,
            Transformation = new Transformation()
                .Width(512)                     // Resize to 512px width
                .Height(512)                    // Resize to 512px height
                .Crop("fill")                   // Crop to fit 512x512
                .Gravity("faces:auto")          // Focus on faces if present, else auto
                .Quality("auto:good")           // Auto-optimize quality
                .FetchFormat("auto")            // Auto-select best format (e.g., WebP)
                .Radius(256)                    // Optional: Circular crop (512px diameter)   
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode == HttpStatusCode.OK)
        {
            return Ok(new
            {
                SecureUrl = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId,
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                Format = uploadResult.Format,
                Bytes = uploadResult.Bytes,
                Version = uploadResult.Version,
                CreatedAt = uploadResult.CreatedAt
            });
        }
        else
        {
            return StatusCode((int)uploadResult.StatusCode, uploadResult.Error?.Message);
        }
    }
}
