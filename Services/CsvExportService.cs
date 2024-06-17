using CronProject.Data;
using CronProject.Models;
using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class CsvExportService
{
    private readonly ApplicationDbContext _context;

    public CsvExportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ExportCsvAsync(string outputPath, Guid importSessionId)
    {
        // Exportar solo los datos con el identificador de sesión de importación especificado
        var groupedRecords = _context.DataRecords
            .Where(r => r.ImportSessionId == importSessionId)
            .GroupBy(r => new { r.RutClienteRecibe, r.NombreClienteRecibe, r.CodigoBancoReceptor,r.NombreBancoReceptor, r.MonedaTransferencia })
            .Select(g => new ExportRecord
            {
                RutClienteRecibe = g.Key.RutClienteRecibe,
                NombreClienteRecibe = g.Key.NombreClienteRecibe,
                CodigoBancoReceptor = g.Key.CodigoBancoReceptor,
                NombreBancoReceptor = g.Key.NombreBancoReceptor,
                MontoTotalTransferencias = g.Sum(r => r.MontoTransferencia),
                MonedaTransferencia = g.Key.MonedaTransferencia
            })
            .ToList();

        using var writer = new StreamWriter(outputPath);
        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

        await csvWriter.WriteRecordsAsync(groupedRecords);
    }
}

public class ExportRecord
{
    public required string RutClienteRecibe { get; set; }
    public required string NombreClienteRecibe { get; set; }
    
    public required string CodigoBancoReceptor { get; set; }
    public required string NombreBancoReceptor { get; set; }
    public decimal MontoTotalTransferencias { get; set; }
    public required string MonedaTransferencia { get; set; }
}
