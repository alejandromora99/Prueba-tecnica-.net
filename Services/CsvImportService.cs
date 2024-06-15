using CronProject.Data;
using CronProject.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CsvImportService
{
    //variables
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly CsvExportService _csvExportService;
    private readonly int _batchSize;

    public CsvImportService(IServiceScopeFactory scopeFactory, CsvExportService csvExportService, int batchSize = 500)
    {
        _scopeFactory = scopeFactory;
        _csvExportService = csvExportService;
        _batchSize = batchSize;
    }

    public async Task ImportCsvAsync(string filePath, Guid importSessionId)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null, // Ignorar la validacion del encabezado
            MissingFieldFound = null,// No lanzar excepción si falta un campo (Por el campo ID)
            PrepareHeaderForMatch = args => args.Header.ToLower(),// Asegurarse de que los encabezados coincidan con los nombres de las propiedades
        };

        // Leo el csv
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);


        csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "dd/MM/yyyy", "yyyy-MM-dd" }; // Configuro CsvHelper con el formato de fecha que tendra el csv (por defecto usa DateTime)

        //cConvierto las filas en lista de objetos
        var records = csv.GetRecords<DataRecordWithoutId>().ToList();

        // Dividir los registros en lotes y procesarlos en paralelo
        var batches = records.Select((record, index) => new { record, index })
                             .GroupBy(x => x.index / _batchSize)
                             .Select(g => g.Select(x => x.record).ToList())
                             .ToList();
//Si batchSize es 100, los indices de 0 a 99 estarab en el primer grupo (clave 0), los índices de 100 a 199 estarán en el segundo grupo (clave 1)

        var tasks = batches.Select(batch => Task.Run(async () =>
        {
            //DbContext  no es seguro para el uso concurrente (trabajando paralelamente)
             // Crear un nuevo alcance (scope) para cada lote
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var dataRecords = batch.Select(record => new DataRecord
            {
                RutClienteEnvia = record.RutClienteEnvia,
                NombreClienteEnvia = record.NombreClienteEnvia,
                IdTransaccion = record.IdTransaccion,
                RutClienteRecibe = record.RutClienteRecibe,
                NombreClienteRecibe = record.NombreClienteRecibe,
                CodigoBancoReceptor = record.CodigoBancoReceptor,
                NombreBancoReceptor = record.NombreBancoReceptor,
                MontoTransferencia = record.MontoTransferencia,
                MonedaTransferencia = record.MonedaTransferencia,
                FechaTransferencia = record.FechaTransferencia,
                ImportSessionId = importSessionId // Asignar el identificador de sesión
            }).ToList();

            await context.DataRecords.AddRangeAsync(dataRecords);
            await context.SaveChangesAsync();
        }));

        await Task.WhenAll(tasks);
    }

    public async Task ImportAndExportCsvAsync(string inputFilePath, string outputFilePath)
    {
        var importSessionId = Guid.NewGuid(); // Generar un identificador de sesión único para esta operación

        await ImportCsvAsync(inputFilePath, importSessionId);
        await _csvExportService.ExportCsvAsync(outputFilePath, importSessionId);
    }
}

public class DataRecordWithoutId
{
    public required string RutClienteEnvia { get; set; }
    public required string NombreClienteEnvia { get; set; }
    public required string IdTransaccion { get; set; }
    public required string RutClienteRecibe { get; set; }
    public required string NombreClienteRecibe { get; set; }
    public required string CodigoBancoReceptor { get; set; }
    public required string NombreBancoReceptor { get; set; }
    public decimal MontoTransferencia { get; set; }
    public required string MonedaTransferencia { get; set; }
    public DateTime FechaTransferencia { get; set; }
}
