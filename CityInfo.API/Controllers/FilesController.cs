using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers;

[Route("api/files")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider fileExtensionContentTypeProvider;

    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        this.fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new ArgumentException(nameof(fileExtensionContentTypeProvider));
    }

    public ActionResult GetFile(string fileId)
    {
        //concept code
        var pathToFile = "test.txt";

        if (!System.IO.File.Exists(pathToFile))
        {
            return NotFound();
        }

        if (!fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType)) 
        {
            contentType = "application/octet-stream";
        }

        var bytes = System.IO.File.ReadAllBytes(pathToFile);

        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }
}
