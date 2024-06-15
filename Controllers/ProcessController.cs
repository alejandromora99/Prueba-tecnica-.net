using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ProcessController : ControllerBase
{
    private readonly CsvImportService _csvImportService;

    public ProcessController(CsvImportService csvImportService)
    {
        _csvImportService = csvImportService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessAndExport([FromQuery] string inputFilePath, [FromQuery] string outputFilePath)
    {
        if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(outputFilePath))
        {
            return BadRequest("Ruta del archivo invalida");
        }

        await _csvImportService.ImportAndExportCsvAsync(inputFilePath, outputFilePath);
        return Ok("Data Procesada y Exportada Correctamente");
    }
}
