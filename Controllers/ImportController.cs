using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    private readonly CsvImportService _csvImportService;

    public ImportController(CsvImportService csvImportService)
    {
        _csvImportService = csvImportService;
    }

    [HttpPost]
    public async Task<IActionResult> Import([FromQuery] string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
        {
            return BadRequest("Ruta del archivo invalida.");
        }

        await _csvImportService.ImportCsvAsync(filePath);
        return Ok("Archivo importado correctamente.");
    }
}
