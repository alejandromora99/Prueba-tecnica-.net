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

        if (!System.IO.File.Exists(inputFilePath))
        {
            return BadRequest($"El archivo de entrada no se encuentra: {inputFilePath}");
        }

        try
        {
            await _csvImportService.ImportAndExportCsvAsync(inputFilePath, outputFilePath);
            return Ok("Datos procesados y exportados correctamente.");
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, $"Error al procesar los datos: {ex.Message}");
        }
    }
}
