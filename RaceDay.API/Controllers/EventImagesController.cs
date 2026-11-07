using System.Security.Claims;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/events/{eventId:int}/image")]
public sealed class EventImagesController : ControllerBase
{
    private readonly RaceDayContext _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EventImagesController> _logger;

    public EventImagesController(RaceDayContext db, IConfiguration configuration, ILogger<EventImagesController> logger)
    {
        _db = db;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>Uploads an event image to Azure Blob Storage and saves its URL. Organiser owner only.</summary>
    [HttpPost]
    [Authorize(Roles = "Organiser")]
    [RequestSizeLimit(8_000_000)]
    public async Task<IActionResult> Upload(int eventId, IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0) return BadRequest(new { message = "Choose an image file." });
        if (file.Length > 8_000_000) return BadRequest(new { message = "Image must be 8 MB or smaller." });
        var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowed.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new { message = "Only JPG, PNG or WebP images are supported." });

        var organiserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var ev = await _db.Events.FirstOrDefaultAsync(e => e.EventID == eventId, cancellationToken);
        if (ev == null) return NotFound(new { message = "Event not found." });
        if (ev.OrganiserID != organiserId) return Forbid();

        var connection = _configuration["AzureBlobStorage:ConnectionString"];
        var containerName = _configuration["AzureBlobStorage:ContainerName"] ?? "event-images";
        if (string.IsNullOrWhiteSpace(connection))
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = "Azure Blob Storage is not configured. Set AzureBlobStorage__ConnectionString." });

        try
        {
            var service = new BlobServiceClient(connection);
            var container = service.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var blob = container.GetBlobClient($"event-{eventId}/{Guid.NewGuid():N}{extension}");
            await using var stream = file.OpenReadStream();
            await blob.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
            }, cancellationToken);

            // A time-limited read-only SAS URL lets MVC render the image while the container stays private.
            ev.ImageUrl = blob.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddYears(1)).ToString();
            await _db.SaveChangesAsync(cancellationToken);
            return Ok(new { url = ev.ImageUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload image for event {EventId}", eventId);
            return StatusCode(StatusCodes.Status502BadGateway, new { message = "The image could not be uploaded to Azure Blob Storage. Check the storage configuration and permissions." });
        }
    }
}
